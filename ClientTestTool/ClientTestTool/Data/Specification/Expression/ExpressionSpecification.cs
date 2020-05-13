///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Specification.Base;

namespace ClientTestTool.Data.Specification.Expression
{
  public sealed class ExpressionSpecification<T> : CompositeSpecification<T>
  {
    public ExpressionSpecification(Func<T, bool> expression)
    {
      mExpression = expression;
    }

    public override bool IsSatisfiedBy(T o)
    {
      return mExpression(o);
    }

    private readonly Func<T, bool> mExpression;
  }
}
