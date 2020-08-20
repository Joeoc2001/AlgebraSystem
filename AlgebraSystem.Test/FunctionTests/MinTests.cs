using Algebra;
using Algebra.Atoms;
using Algebra.Functions;
using Algebra.Functions.HardcodedFunctionIdentities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FunctionTests
{
    class MinTests
    {
        [Test]
        public void Min_Type_IsFunction()
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act

            // Assert
            Assert.IsInstanceOf(typeof(Function), expression);
        }

        [Test]
        public void Min_Identity_IsMinIdentity()
        {
            // Arrange
            Function function = (Function)MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act

            // Assert
            Assert.AreEqual(MinIdentity.Instance, function.GetIdentity());
        }

        [Test]
        public void Min_Name_IsMin()
        {
            // Arrange
            Function function = (Function)MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act

            // Assert
            Assert.AreEqual("min", function.GetIdentity().GetName());
        }

        [Test]
        public void Min_InstancesXMinY_AreEqual([Values] EqualityLevel depth)
        {
            // Arrange
            Function function1 = (Function)MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Function function2 = (Function)MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act
            bool equal = function1.Equals(function2, depth);

            // Assert
            Assert.IsTrue(equal);
        }

        [Test]
        public void Min_InstancesXMinY_AreNotEqual_IfCommuted([Values] EqualityLevel depth)
        {
            // Arrange
            Function function1 = (Function)MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Function function2 = (Function)MinIdentity.Instance.CreateExpression(Variable.Y, Variable.X);

            // Act
            bool equal = function1.Equals(function2, depth);

            // Assert
            Assert.IsFalse(equal);
        }

        [Test]
        public void Min_InstancesXMinY_ParametersInListAreCorrect([Values(0, 1)] int parameterIndex)
        {
            // Arrange
            Function function = (Function)MinIdentity.Instance.CreateExpression(Constant.From(0), Constant.From(1));

            // Act
            Expression parameter = function.GetParameterList()[parameterIndex];
            Expression expected = Constant.From(parameterIndex);

            // Assert
            Assert.AreEqual(expected, parameter);
        }

        [Test]
        public void Min_InstancesXMinY_AtomicFormIsCorrect()
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Expression expected = 0.5 * (Variable.X + Variable.Y - (Variable.X - Variable.Y) * Expression.SignOf(Variable.X - Variable.Y));

            // Act
            Expression atomic = expression.GetAtomicExpression();
            bool areExactlyEqual = expected.Equals(atomic, EqualityLevel.Exactly);

            // Assert
            Assert.IsTrue(areExactlyEqual);
        }

        [Test]
        public void Min_InstancesXMinX_AtomicFormIsX()
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.X);
            Expression expected = Variable.X;

            // Act
            Expression atomic = expression.GetAtomicExpression();
            bool areExactlyEqual = expected.Equals(atomic, EqualityLevel.Exactly);

            // Assert
            Assert.IsTrue(areExactlyEqual);
        }

        [Test]
        public void Min_InstancesXMinX_NonAtomicFormIsNotX()
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.X);
            Expression expected = Variable.X;

            // Act
            bool areExactlyEqual = expected.Equals(expression, EqualityLevel.Exactly);

            // Assert
            Assert.IsFalse(areExactlyEqual);
        }

        [Test]
        public void Min_InstancesXMinX_AtomicEqualsX()
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.X);
            Expression expected = Variable.X;

            // Act
            bool areExactlyEqual = expected.Equals(expression, EqualityLevel.Atomic);

            // Assert
            Assert.IsTrue(areExactlyEqual);
        }

        [Test]
        public void Min_InstancesXMinX_NonSpecifiedIsNotX()
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.X);
            Expression expected = Variable.X;

            // Act
            bool areExactlyEqual = expected.Equals(expression);

            // Assert
            Assert.IsFalse(areExactlyEqual);
        }

        [Test]
        public void Min_InstancesXMinY_EvaluatesCorrectly([Range(-10, 10)] int x, [Range(-10, 10)] int y)
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act
            float result = expression.GetDelegate(new VariableInputSet(new Vector2(x, y)))();

            // Assert
            Assert.AreEqual(Math.Min(x, y), result);
        }

        [Test]
        public void Min_InstancesXMinY_MapMapsParameters()
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Expression expected = MinIdentity.Instance.CreateExpression(Variable.Z, Variable.Z);

            // Act
            Expression result = expression.PostMap(e => e is Variable v ? Variable.Z : e);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Min_InstancesXMinY_MapDoesntChangeOriginal()
        {
            // Arrange
            Expression expression = MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Expression expected = MinIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act
            expression.PostMap(e => e is Variable v ? Variable.Z : e);

            // Assert
            Assert.AreEqual(expected, expression);
        }
    }
}
