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
    class DivTests
    {
        [Test]
        public void Div_Type_IsFunction()
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act

            // Assert
            Assert.IsInstanceOf(typeof(Function), expression);
        }

        [Test]
        public void Div_Identity_IsDivIdentity()
        {
            // Arrange
            Function function = (Function)DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act

            // Assert
            Assert.AreEqual(DivIdentity.Instance, function.GetIdentity());
        }

        [Test]
        public void Div_Name_IsDiv()
        {
            // Arrange
            Function function = (Function)DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act

            // Assert
            Assert.AreEqual("div", function.GetIdentity().GetName());
        }

        [Test]
        public void Div_InstancesXDivY_AreEqual([Values] EqualityLevel depth)
        {
            // Arrange
            Function function1 = (Function)DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Function function2 = (Function)DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act
            bool equal = function1.Equals(function2, depth);

            // Assert
            Assert.IsTrue(equal);
        }

        [Test]
        public void Div_InstancesXDivY_AreNotEqual_IfCommuted([Values] EqualityLevel depth)
        {
            // Arrange
            Function function1 = (Function)DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Function function2 = (Function)DivIdentity.Instance.CreateExpression(Variable.Y, Variable.X);

            // Act
            bool equal = function1.Equals(function2, depth);

            // Assert
            Assert.IsFalse(equal);
        }

        [Test]
        public void Div_InstancesXDivY_ParametersInListAreCorrect([Values(0, 1)] int parameterIndex)
        {
            // Arrange
            Function function = (Function)DivIdentity.Instance.CreateExpression(Constant.From(0), Constant.From(1));

            // Act
            Expression parameter = function.GetParameterList()[parameterIndex];
            Expression expected = Constant.From(parameterIndex);

            // Assert
            Assert.AreEqual(expected, parameter);
        }

        [Test]
        public void Div_InstancesXDivY_AtomicFormIsCorrect()
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Expression expected = Variable.X * Expression.Pow(Variable.Y, -1);

            // Act
            Expression atomic = expression.GetAtomicExpression();
            bool areExactlyEqual = expected.Equals(atomic, EqualityLevel.Exactly);

            // Assert
            Assert.IsTrue(areExactlyEqual);
        }

        [Test]
        public void Div_InstancesXDivX_AtomicFormIs1()
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.X);
            Expression expected = 1;

            // Act
            Expression atomic = expression.GetAtomicExpression();
            bool areExactlyEqual = expected.Equals(atomic, EqualityLevel.Exactly);

            // Assert
            Assert.IsTrue(areExactlyEqual);
        }

        [Test]
        public void Div_InstancesXDivX_NonAtomicFormIsNot1()
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.X);
            Expression expected = 1;

            // Act
            bool areExactlyEqual = expected.Equals(expression, EqualityLevel.Exactly);

            // Assert
            Assert.IsFalse(areExactlyEqual);
        }

        [Test]
        public void Div_InstancesXDivX_AtomicEquals1()
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.X);
            Expression expected = 1;

            // Act
            bool areExactlyEqual = expected.Equals(expression, EqualityLevel.Atomic);

            // Assert
            Assert.IsTrue(areExactlyEqual);
        }

        [Test]
        public void Div_InstancesXDivX_NonSpecifiedEquals1()
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.X);
            Expression expected = 1;

            // Act
            bool areExactlyEqual = expected.Equals(expression);

            // Assert
            Assert.IsTrue(areExactlyEqual);
        }

        [Test]
        public void Div_InstancesXDivY_EvaluatesCorrectly([Range(-10, 10)] int x, [Range(-10, 10)] int y)
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act
            float result = expression.GetDelegate(new VariableInputSet(new Vector2(x, y)))();

            // Assert
            Assert.That(result, Is.EqualTo((float)x / y).Within(0.00000001f));
        }

        [Test]
        public void Div_InstancesXDivY_MapMapsParameters()
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Expression expected = DivIdentity.Instance.CreateExpression(Variable.Z, Variable.Z);

            // Act
            Expression result = expression.Map(e => e is Variable v ? Variable.Z : e);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Div_InstancesXDivY_MapDoesntChangeOriginal()
        {
            // Arrange
            Expression expression = DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);
            Expression expected = DivIdentity.Instance.CreateExpression(Variable.X, Variable.Y);

            // Act
            expression.Map(e => e is Variable v ? Variable.Z : e);

            // Assert
            Assert.AreEqual(expected, expression);
        }
    }
}
