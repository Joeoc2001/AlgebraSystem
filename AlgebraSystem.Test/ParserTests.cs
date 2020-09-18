using System.Collections;
using System.Collections.Generic;
using System.IO;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;
using NUnit.Framework;
using Rationals;

namespace AlgebraTests
{
    public class ParserTests
    {
        [Test]
        public void Parser_ParsesVariables([Values("X", "Y", "Z")] string name)
        {
            // ARANGE
            string expression = name;
            Expression expected = Expression.VariableFrom(name);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesConstants()
        {
            // ARANGE
            string expression = "987654321.5";
            Expression expected = Expression.ConstantFrom((Rational)987654321.5M);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesFractionConstants()
        {
            // ARANGE
            string expression = "98765/24";
            Expression expected = Expression.ConstantFrom((Rational)98765 / 24);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesAddition()
        {
            // ARANGE
            string expression = "x + 1";
            Expression expected = Expression.VarX + 1;

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMultiplication()
        {
            // ARANGE
            string expression = "x * y";
            Expression expected = Expression.VarX * Expression.VarY;

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSubtraction()
        {
            // ARANGE
            string expression = "x - 50";
            Expression expected = Expression.VarX + (-50);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesDivision()
        {
            // ARANGE
            string expression = "x / y";
            Expression expected = Expression.VarX / Expression.VarY;

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesExponentiation()
        {
            // ARANGE
            string expression = "x ^ y";
            Expression expected = Expression.Pow(Expression.VarX, Expression.VarY);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesBraces()
        {
            // ARANGE
            string expression = "(x + y) * 5";
            Expression expected = (Expression.VarX + Expression.VarY) * 5;

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSingleNegation()
        {
            // ARANGE
            string expression = "-5";
            Expression expected = Expression.ConstantFrom(-1) * Expression.ConstantFrom(5);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesDoubleNegation()
        {
            // ARANGE
            string expression = "--5";
            Expression expected = Expression.ConstantFrom(-1) * (Expression.ConstantFrom(-1) * Expression.ConstantFrom(5));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLnBraceless()
        {
            // ARANGE
            string expression = "ln 5";
            Expression expected = Expression.LnOf(Expression.ConstantFrom(5));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLnBraces()
        {
            // ARANGE
            string expression = "ln(52)";
            Expression expected = Expression.LnOf(Expression.ConstantFrom(52));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLog()
        {
            // ARANGE
            string expression = "Log (y, x) ";
            Expression expected = Expression.LogOf(Expression.VarY, Expression.VarX);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLogCapital()
        {
            // ARANGE
            string expression = "LOG (152, x) ";
            Expression expected = Expression.LogOf(Expression.ConstantFrom(152), Expression.VarX);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedLnBraceless()
        {
            // ARANGE
            string expression = "ln ln 15";
            Expression expected = Expression.LnOf(Expression.LnOf(Expression.ConstantFrom(15)));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLnBracelessAsOnlyNextLeaf()
        {
            // ARANGE
            string expression = "y * ln 5 * x + 3";
            Expression expected = (Expression.VarY * Expression.LnOf(5) * Expression.VarX) + 3;

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSignBraceless()
        {
            // ARANGE
            string expression = "sign 5";
            Expression expected = Expression.SignOf(Expression.ConstantFrom(5));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSignBraces()
        {
            // ARANGE
            string expression = "sign(529)";
            Expression expected = Expression.SignOf(Expression.ConstantFrom(529));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedSignBraceless()
        {
            // ARANGE
            string expression = "sign sign 145";
            Expression expected = Expression.SignOf(Expression.SignOf(Expression.ConstantFrom(145)));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedSignBraces()
        {
            // ARANGE
            string expression = "sign(sign(1555))";
            Expression expected = Expression.SignOf(Expression.SignOf(Expression.ConstantFrom(1555)));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinConstants()
        {
            // ARANGE
            string expression = "min(1, 2)";
            Expression expected = Expression.Min(1, 2);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinVariables()
        {
            // ARANGE
            string expression = "min(x, y)";
            Expression expected = Expression.Min(Expression.VarX, Expression.VarY);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinAddition()
        {
            // ARANGE
            string expression = "min(x + y, y)";
            Expression expected = Expression.Min(Expression.VarX + Expression.VarY, Expression.VarY);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinMultiplication()
        {
            // ARANGE
            string expression = "min(x * y, y * z)";
            Expression expected = Expression.Min(Expression.VarX * Expression.VarY, Expression.VarY * Expression.VarZ);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxConstants()
        {
            // ARANGE
            string expression = "max(1, 2)";
            Expression expected = Expression.Max(1, 2);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxVariables()
        {
            // ARANGE
            string expression = "max(x, y)";
            Expression expected = Expression.Max(Expression.VarX, Expression.VarY);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxAddition()
        {
            // ARANGE
            string expression = "max(x + y, y)";
            Expression expected = Expression.Max(Expression.VarX + Expression.VarY, Expression.VarY);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxMultiplication()
        {
            // ARANGE
            string expression = "max(x * y, y * z)";
            Expression expected = Expression.Max(Expression.VarX * Expression.VarY, Expression.VarY * Expression.VarZ);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinMaxMultiplication()
        {
            // ARANGE
            string expression = "min (1, x) * max(x * y, y * z)";
            Expression expected = Expression.Min(1, Expression.VarX) * Expression.Max(Expression.VarX * Expression.VarY, Expression.VarY * Expression.VarZ);

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedBraces()
        {
            // ARANGE
            string expression = "(((15)))";
            Expression expected = 15;

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces1()
        {
            // ARANGE
            string expression = "(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces2()
        {
            // ARANGE
            string expression = "12(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces3()
        {
            // ARANGE
            string expression = "(192)(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces4()
        {
            // ARANGE
            string expression = "((1223)";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnAdditionWithoutLHS()
        {
            // ARANGE
            string expression = "+ 9";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnAdditionWithoutRHS()
        {
            // ARANGE
            string expression = "1 +";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMultiplicationWithoutLHS()
        {
            // ARANGE
            string expression = "* 8";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMultiplicationWithoutRHS()
        {
            // ARANGE
            string expression = "17 *";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnExponentiationWithoutLHS()
        {
            // ARANGE
            string expression = "^ x";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnExponentiationWithoutRHS()
        {
            // ARANGE
            string expression = "x ^";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnSubtractionWithoutRHS()
        {
            // ARANGE
            string expression = "x -";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnDivisionWithoutLHS()
        {
            // ARANGE
            string expression = "/ x";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnDivisionWithoutRHS()
        {
            // ARANGE
            string expression = "x /";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnLnWithoutArguments()
        {
            // ARANGE
            string expression = "x + ln";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnLnWithTooManyArguments()
        {
            // ARANGE
            string expression = "7 + ln(x, y)";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMinWithoutBraces()
        {
            // ARANGE
            string expression = "7 + min x, y";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ParsesDifferentFunctions()
        {
            // ARANGE
            string expression = "sign(ln 3883)";
            Expression expected = Expression.SignOf(Expression.LnOf(Expression.ConstantFrom(3883)));

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure1()
        {
            // ARANGE
            Expression expected = Expression.Pow(Expression.VarY, (Rational)3971 / 9748);
            string expression = "((y ^ 3971/9748))";

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure2()
        {
            // ARANGE
            Expression expected = (Expression.Pow(((Expression.VarX + Expression.VarZ + Expression.ConstantFrom((Rational)(18162171) / (22852115))) * Expression.ConstantFrom((Rational)(310824508140) / (806613624271)) * Expression.Pow(Expression.Pow(Expression.Pow(Expression.ConstantFrom((Rational)(3271) / (5692)), Expression.Pow(Expression.ConstantFrom((Rational)(3875) / (6319)), Expression.VarZ)), Expression.LnOf((Expression.VarY * Expression.VarZ * Expression.ConstantFrom((Rational)(7876848) / (551214185))))), Expression.ConstantFrom((Rational)(7481) / (15820))) * Expression.ConstantFrom((Rational)(1077) / (2105)) * Expression.LnOf(Expression.Pow(Expression.ConstantFrom((Rational)(-31938485) / (179632821)), Expression.ConstantFrom((Rational)(3719) / (3898)))) * Expression.Pow(Expression.ConstantFrom((Rational)(34060164457463332) / (84917266513976971)), Expression.ConstantFrom((Rational)(7170) / (8081)))), Expression.ConstantFrom((Rational)(3757) / (7268))) + Expression.ConstantFrom((Rational)(2704) / (8117)) + Expression.Pow((Expression.Pow(Expression.VarZ, Expression.Pow(((Expression.VarY * Expression.ConstantFrom((Rational)(281756499029) / (4278581823008))) + Expression.VarX + Expression.LnOf(Expression.Pow(Expression.ConstantFrom((Rational)(107) / (91937)), Expression.ConstantFrom((Rational)(23485) / (60351)))) + Expression.ConstantFrom((Rational)(241903) / (1023370))), Expression.Pow(Expression.ConstantFrom((Rational)(-84694382) / (48380467)), Expression.ConstantFrom((Rational)(132483) / (137719))))) + Expression.Pow(Expression.Pow((Expression.Pow(Expression.ConstantFrom((Rational)(3445) / (5332)), Expression.ConstantFrom((Rational)(2774) / (27105))) * Expression.ConstantFrom((Rational)(1990944) / (93080449))), (Expression.VarX + Expression.VarY + Expression.ConstantFrom((Rational)(9777) / (10613)) + (Expression.Pow(Expression.ConstantFrom((Rational)(17643) / (49516)), (Expression.VarY * Expression.ConstantFrom((Rational)(1172263) / (22515807)))) * Expression.Pow(Expression.VarZ, Expression.ConstantFrom((Rational)(1262) / (8407))) * Expression.ConstantFrom((Rational)(3121) / (3977))))), Expression.ConstantFrom((Rational)(-56263742) / (634120667))) + Expression.Pow(Expression.Pow(Expression.Pow(Expression.VarX, Expression.Pow(Expression.ConstantFrom((Rational)(6957) / (15283)), Expression.VarY)), Expression.ConstantFrom((Rational)(2435) / (4603))), Expression.ConstantFrom((Rational)(90859500) / (183100176133))) + Expression.Pow(Expression.Pow(Expression.Pow(Expression.LnOf(((Expression.VarX + Expression.ConstantFrom((Rational)(1684097) / (2595208))) * Expression.Pow(Expression.ConstantFrom((Rational)(3541) / (4870)), Expression.VarZ) * Expression.ConstantFrom((Rational)(-351601137) / (516062869)))), Expression.LnOf((Expression.ConstantFrom((Rational)(520875825076) / (626696076699)) + Expression.Pow(Expression.ConstantFrom((Rational)(493) / (7544)), Expression.ConstantFrom((Rational)(4035) / (4984)))))), (Expression.Pow(Expression.VarY, Expression.VarY) * Expression.Pow(Expression.LnOf(Expression.Pow(Expression.ConstantFrom((Rational)(1845) / (4184)), Expression.ConstantFrom((Rational)(4390) / (5839)))), Expression.ConstantFrom((Rational)(529) / (9484))))), Expression.ConstantFrom((Rational)(35351) / (78776))) + Expression.Pow(Expression.VarX, Expression.VarZ) + Expression.ConstantFrom((Rational)(9241) / (17113)) + Expression.Pow(Expression.ConstantFrom((Rational)(4733) / (11013)), Expression.VarY) + Expression.Pow(Expression.LnOf(Expression.Pow(Expression.VarY, Expression.VarZ)), Expression.Pow(Expression.ConstantFrom((Rational)(8413) / (10191)), Expression.LnOf(Expression.Pow(Expression.ConstantFrom((Rational)(1784) / (7051)), Expression.Pow(Expression.VarX, Expression.VarY))))) + Expression.ConstantFrom((Rational)(3572896512839) / (32129243331984))), Expression.ConstantFrom((Rational)(4071) / (4378))));
            string expression = "((((x + z + 18162171/22852115) * 310824508140/806613624271 * (((3271/5692 ^ (3875/6319 ^ z)) ^ ln (y * z * 7876848/551214185)) ^ 7481/15820) * 1077/2105 * ln (-31938485/179632821 ^ 3719/3898) * (34060164457463332/84917266513976971 ^ 7170/8081)) ^ 3757/7268) + 2704/8117 + (((z ^ (((y * 281756499029/4278581823008) + x + ln (107/91937 ^ 23485/60351) + 241903/1023370) ^ (-84694382/48380467 ^ 132483/137719))) + ((((3445/5332 ^ 2774/27105) * 1990944/93080449) ^ (x + y + 9777/10613 + ((17643/49516 ^ (y * 1172263/22515807)) * (z ^ 1262/8407) * 3121/3977))) ^ -56263742/634120667) + (((x ^ (6957/15283 ^ y)) ^ 2435/4603) ^ 90859500/183100176133) + (((ln ((x + 1684097/2595208) * (3541/4870 ^ z) * -351601137/516062869) ^ ln (520875825076/626696076699 + (493/7544 ^ 4035/4984))) ^ ((y ^ y) * (ln (1845/4184 ^ 4390/5839) ^ 529/9484))) ^ 35351/78776) + (x ^ z) + 9241/17113 + (4733/11013 ^ y) + (ln (y ^ z) ^ (8413/10191 ^ ln (1784/7051 ^ (x ^ y)))) + 3572896512839/32129243331984) ^ 4071/4378))";

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure3()
        {
            // ARANGE
            Expression expected = (Expression.VarZ * Expression.Pow(Expression.VarY, Expression.ConstantFrom((Rational)(2) / (1))) * Expression.SignOf((Expression.LnOf(Expression.ConstantFrom((Rational)(4971) / (5138))) + Expression.ConstantFrom((Rational)(3996) / (4441)) + Expression.Pow(Expression.VarX, Expression.Pow(Expression.ConstantFrom((Rational)(2401) / (4209)), Expression.SignOf(Expression.Pow(Expression.LnOf(Expression.Pow(Expression.ConstantFrom((Rational)(39449) / (47989)), Expression.SignOf(Expression.VarZ))), Expression.Pow(((Expression.VarZ + Expression.ConstantFrom((Rational)(815) / (6387)) + Expression.VarY) * Expression.ConstantFrom((Rational)(1894) / (5563))), Expression.ConstantFrom((Rational)(13524) / (15523))))))) + Expression.LnOf(Expression.SignOf(Expression.ConstantFrom((Rational)(1368) / (19661)))))));
            string expression = "(z * (y ^ 2) * sign (ln 4971/5138 + 3996/4441 + (x ^ (2401/4209 ^ sign (ln (39449/47989 ^ sign z) ^ (((z + 815/6387 + y) * 1894/5563) ^ 13524/15523)))) + ln sign 1368/19661))";

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }
    }
    
}
