using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTT_CovarageMap
{
    public class ProfileFeature
    {

        public ProfileFeature(int row, String featureID)
        {
            Row = row;
            FeatureID = featureID;
        }

        int _row;

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }
        String _featureID;

        public String FeatureID
        {
            get { return _featureID; }
            set { _featureID = value; }
        }

    }
}
