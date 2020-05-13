using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestTool.Tests.Definitions.Data;

namespace TestTool.Tests.Engine.Data
{
    internal class NodeType
    {
        public NodeType(bool isExecuted, TestInfo test)
        {
            IsExecuted = isExecuted;
            Test = test;
        }

        public bool IsExecuted;
        public TestInfo Test;
    }



    public class ExecutableTestList: IEnumerable<TestInfo>
    {

        public ExecutableTestList()
        {}

        public ExecutableTestList(IEnumerable<TestInfo> testInfoList)
        {
            AddRange(testInfoList);
        }

        public void Add(TestInfo newTest)
        {
            try
            {
                m_Lock.AcquireWriterLock(int.MaxValue);

                InternalAdd(newTest);

                if (null != RestartEnumeration)
                    RestartEnumeration();
            }
            finally
            {
                m_Lock.ReleaseWriterLock();
            }
        }

        public void AddRange(IEnumerable<TestInfo> newTests)
        {
            try
            {
                m_Lock.AcquireWriterLock(int.MaxValue);

                foreach (var testInfo in newTests)
                {
                    InternalAdd(testInfo);
                }

                if (null != RestartEnumeration)
                    RestartEnumeration();
            }
            finally
            {
                m_Lock.ReleaseWriterLock();
            }
        }

        private void InternalAdd(TestInfo newTest)
        {
            var s = m_TestsList.FirstOrDefault(e => e.Test == newTest);
            if (null != s)
                throw new ArgumentException(string.Format("The test with name '{0}' and ID '{1}' is in the executable list already", newTest.Name, newTest.Id));

            var v = new NodeType(false, newTest);
            var before = FindNodeBefore(newTest);
            if (null == before)
                m_TestsList.AddFirst(v);
            else
                m_TestsList.AddAfter(before, new LinkedListNode<NodeType>(v));
        }

        public void Remove(TestInfo T)
        {
            if (T == m_TestUnderExecution)
                throw new ArgumentException("Can't delete test under execution");

            try
            {
                m_Lock.AcquireWriterLock(int.MaxValue);

                var nodeToRemove = m_TestsList.FirstOrDefault(e => e.Test == T);
                if (null != nodeToRemove)
                    m_TestsList.Remove(nodeToRemove);
            }
            finally
            {
                m_Lock.ReleaseWriterLock();
            }
        }

        public void Clear()
        {
            m_TestsList.Clear();
        }

        public void Prepare()
        {
            m_TestsList = new LinkedList<NodeType>(m_TestsList.OrderBy(e => e.Test.ExecutionOrder).ThenBy(e => e.Test.Category).ThenBy(e => e.Test.Order));
        }

        public void StartExecution()
        {
            m_UnderExecution = true;
            foreach (var nodeType in m_TestsList)
            { nodeType.IsExecuted = false; }
        }

        public void EndExecution()
        {
            m_UnderExecution = false;
            m_TestUnderExecution = null;
        }

        #region Properties

        public int Count 
        {
            get { return m_TestsList.Count; }
        }

        public TestInfo TestUnderExecution
        {
            get { return m_TestUnderExecution; }
        }

        #endregion

        #region Events
        protected event Action RestartEnumeration = null;
        #endregion

        //Inserts new node preserving order according ExecutionOrder field
        private LinkedListNode<NodeType> FindNodeBefore(TestInfo T)
        {
            if (!m_TestsList.Any())
                return null;

            if (T.ExecutionOrder < m_TestsList.First().Test.ExecutionOrder)
                return null;

            LinkedListNode<NodeType> r = null;
            LinkedListNode<NodeType> current = m_TestsList.First;
            while (null != current)
            {
                if (current.Value.Test.ExecutionOrder == T.ExecutionOrder)
                    r = current;
                current = current.Next;
            }

            return r ?? m_TestsList.Last;
        }

        //If the list is modified during iteration over this enumerator then
        //enumerator automatically restarts from the beginning.
        //After restarting the iterator processes only elements with IsExecuted == false.
        //It allows correctly add tests with high ExecutionOrder priority.
        public IEnumerator<TestInfo> GetUpdatableEnumerator()
        {
            if (m_TestsList.Any())
            {
                var restartEnumeration = true;
                Action restartEnumerationAction = () => restartEnumeration = true;
                try
                {
                    LinkedListNode<NodeType> current = null;

                    this.RestartEnumeration += restartEnumerationAction;

                    do
                    {
                        m_Lock.AcquireWriterLock(int.MaxValue);

                        if (restartEnumeration)
                        {
                            if (!m_TestsList.Any())
                                yield break;
                                
                            current = m_TestsList.First;
                            restartEnumeration = false;
                        }

                        if (!current.Value.IsExecuted)
                        {
                            if (m_UnderExecution)
                            {
                                current.Value.IsExecuted = true;
                                m_TestUnderExecution = current.Value.Test;
                            }
                            m_Lock.ReleaseWriterLock();

                            yield return current.Value.Test;
                        }
                        else
                            m_Lock.ReleaseWriterLock();

                        current = current.Next;
                    } while (null != current);

                }
                finally
                {
                    this.RestartEnumeration -= restartEnumerationAction;
                    m_TestUnderExecution = null;
                }
            }
        }

        public IEnumerator<TestInfo> GetEnumerator()
        {
            return m_TestsList.Select(e => e.Test).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool m_UnderExecution;
        private readonly ReaderWriterLock m_Lock = new ReaderWriterLock();
        private TestInfo m_TestUnderExecution;
        private LinkedList<NodeType> m_TestsList = new LinkedList<NodeType>();
    }
}
