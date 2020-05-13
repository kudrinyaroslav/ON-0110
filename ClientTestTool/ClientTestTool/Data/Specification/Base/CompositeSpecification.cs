///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Specification.Interfaces;

namespace ClientTestTool.Data.Specification.Base
{
  public abstract class CompositeSpecification<T> : ISpecification<T>
  {
    public abstract bool IsSatisfiedBy(T o);

    public ISpecification<T> And(ISpecification<T> specification)
    {
      return new AndSpecification<T>(this, specification);
    }
    public ISpecification<T> Or(ISpecification<T> specification)
    {
      return new OrSpecification<T>(this, specification);
    }
    public ISpecification<T> Not(ISpecification<T> specification)
    {
      return new NotSpecification<T>(specification);
    }

    private class AndSpecification<T> : CompositeSpecification<T>
    {
      public AndSpecification(ISpecification<T> left, ISpecification<T> right)
      {
        mLeft  = left;
        mRight = right;
      }

      public override bool IsSatisfiedBy(T o)
      {
        return mLeft.IsSatisfiedBy(o) && mRight.IsSatisfiedBy(o);
      }

      private readonly ISpecification<T> mLeft;
      private readonly ISpecification<T> mRight;
    }

    private class OrSpecification<T> : CompositeSpecification<T>
    {
      public OrSpecification(ISpecification<T> left, ISpecification<T> right)
      {
        mLeft  = left;
        mRight = right;
      }

      public override bool IsSatisfiedBy(T o)
      {
        return mLeft.IsSatisfiedBy(o) || mRight.IsSatisfiedBy(o);
      }

      private readonly ISpecification<T> mLeft;
      private readonly ISpecification<T> mRight;
    }

    private class NotSpecification<T> : CompositeSpecification<T>
    {
      public NotSpecification(ISpecification<T> specification)
      {
        mSpecification = specification;
      }

      public override bool IsSatisfiedBy(T o)
      {
        return !mSpecification.IsSatisfiedBy(o);
      }

      private readonly ISpecification<T> mSpecification;
    }
  }



}
