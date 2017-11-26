using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMLMappingCSTests;
using Azuki.UML;
using System.Collections.Generic;

namespace UMLMappingCSTest
{
    [TestClass]
    public class AssociationTests
    {
        ///
        /// 生成のテスト
        ///

        private static void CheckOrdered(FixedAssociation<Hogera> association)
        {
            Hogera preObj = null;
            foreach (Hogera obj in association.Ends)
            {
                if( preObj != null)
                {
                    if( preObj.Name.CompareTo(obj.Name) > 0)
                    {
                        Assert.AreEqual(obj.Name, preObj.Name);
                    }
                }
                preObj = obj;
            }
        }

        private static void CheckOrdered(Association<Hogera> association)
        {
            Hogera preObj = null;
            foreach (Hogera obj in association.Ends)
            {
                if (preObj != null)
                {
                    if (preObj.Name.CompareTo(obj.Name) > 0)
                    {
                        Assert.AreEqual(obj.Name, preObj.Name);
                    }
                }
                preObj = obj;
            }
        }

        private static void CheckNotOrdered(Association<Hogera> association)
        {
            bool notOrdered = false;
            try
            {
                CheckOrdered(association);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                notOrdered = true;
            }
            Assert.IsTrue(notOrdered, "順序通りになっています");
        }

        private static void CheckNotOrdered(FixedAssociation<Hogera> association)
        {
            bool notOrdered = false;
            try
            {
                CheckOrdered(association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                notOrdered = true;
            }
            Assert.IsTrue(notOrdered, "順序通りになっています");
        }

        [TestMethod]
        public void TestCase1_1_1()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1   1   1   生成 なし   3..6    あり あり  下限 あり  NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_1(agreeKind);
        }

        private static void TestCase1_X_1(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>();
            Hogera x = new Hogera("No2");
            list.Add(x);
            list.Add(new Hogera("No3"));
            list.Add(x);
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, true, true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsTrue(exception); // NGならば、テストとしてはOK。
        }

        [TestMethod]
        public void TestCase1_1_2()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	2		なし  0..5	なし なし  上限+1	なし NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_2(agreeKind);
        }

        private static void TestCase1_X_2(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No6"),
                new Hogera("No5"),
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2"),
                new Hogera("No1")
            };
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, 5, list, false, false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsTrue(exception); // NGならば、テストとしてはOK。
        }

        [TestMethod]
        public void TestCase1_1_3()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	3		なし  3	なし あり  下限 なし  OK,3, YES
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_3(agreeKind);
        }

        private static void TestCase1_X_3(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3"),
                new Hogera("No2"),
                new Hogera("No1")
            };
            bool exception = false;
            try
            {
                FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, true, "Name");
                foreach(Hogera hogera in association.Ends)
                {
                    hogera.Func();
                }
                Assert.AreEqual(3, association.Count);

                CheckOrdered(association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception); // OK
        }

        [TestMethod]
        public void TestCase1_1_4()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	4		なし  3..6	あり なし  下限-1	なし NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_4(agreeKind);
        }

        private static void TestCase1_X_4(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No2"),
                new Hogera("No1")
            };
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsTrue(exception); //  NGならば、テストとしてはOK。
        }

        [TestMethod]
        public void TestCase1_1_5()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	5		なし  0..5	あり あり  上限 あり  NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_5(agreeKind);
        }

        private static void TestCase1_X_5(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>();
            Hogera x = new Hogera("No2");
            list.Add(x);
            list.Add(new Hogera("No3"));
            list.Add(x);
            list.Add(new Hogera("No5"));
            list.Add(new Hogera("No4"));
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, 5, list, true, true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsTrue(exception); // NGならば、テストとしてはOK。
        }

        [TestMethod]
        public void TestCase1_1_6()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	6		なし  3	なし なし  下限-1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_6(agreeKind);
        }

        private static void TestCase1_X_6(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>();
            Hogera obj = new Hogera("No2");
            list.Add(obj);
            list.Add(obj);
            bool exception = false;
            try
            {
                FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsTrue(exception); // NGならば、テストとしてはOK。
        }

        [TestMethod]
        public void TestCase1_1_7()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	7		なし  3..6	なし なし  上限 なし  OK,6, NO
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_7(agreeKind);
        }

        private static void TestCase1_X_7(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3"),
                new Hogera("No5"),
                new Hogera("No4"),
                new Hogera("No1"),
                new Hogera("No6"),
                new Hogera("No2")
            };
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, false, false);
                Assert.AreEqual(6, association.Count);

                CheckNotOrdered(association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception); // OK
        }

        [TestMethod]
        public void TestCase1_1_8()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	8		なし  3	なし あり  上限 なし  OK,3, YES
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_8(agreeKind);
        }

        private static void TestCase1_X_8(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3"),
                new Hogera("No1"),
                new Hogera("No2")
            };
            bool exception = false;
            try
            {
                FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, true, "Name");
                Assert.AreEqual(3, association.Count);

                CheckOrdered(association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception); // OK
        }

        [TestMethod]
        public void TestCase1_1_9()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	9		なし  0..1	なし あり  上限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_9(agreeKind);
        }

        private static void TestCase1_X_9(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, new Hogera("No1"));
                Assert.AreEqual(1, association.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception); // OK
        }

        [TestMethod]
        public void TestCase1_1_10()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	10		なし  1	なし なし  上限+1	なし NG
            // 多重度1固定なので、初期要素数が上限+1にはできず、ありえない。
        }

        [TestMethod]
        public void TestCase1_1_11()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	11		なし  1	なし なし  上限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_11(agreeKind);
        }

        private static void TestCase1_X_11(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, new Hogera("No1"));
                Assert.AreEqual(1, association.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception); // OK
        }

        [TestMethod]
        public void TestCase1_1_12()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	12		なし  3..6	あり あり  上限+1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_12(agreeKind);
        }

        private static void TestCase1_X_12(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No7"),
                new Hogera("No3"),
                new Hogera("No5"),
                new Hogera("No4"),
                new Hogera("No1"),
                new Hogera("No6"),
                new Hogera("No2")
            };
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, false, false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsTrue(exception); // NGならば、テストとしてはOK。
        }

        [TestMethod]
        public void TestCase1_1_13()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	13		なし  0..1	あり なし  下限 なし  OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_13(agreeKind);
        }

        private static void TestCase1_X_13(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind);
                Assert.AreEqual(0, association.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception); // OK
        }

        [TestMethod]
        public void TestCase1_1_14()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	14		なし  1	なし なし  下限-1	なし NG
            // 多重度1固定なので、初期要素数が下限-1にはできず、ありえない。
        }

        [TestMethod]
        public void TestCase1_1_15()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	15		なし  3	なし なし  上限+1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_15(agreeKind);
        }

        private static void TestCase1_X_15(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3"),
                new Hogera("No1"),
                new Hogera("No4"),
                new Hogera("No2")
            };
            bool exception = false;
            try
            {
                FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsTrue(exception); // NGならば、テストとしてはOK。
        }

        [TestMethod]
        public void TestCase1_1_16()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	16		なし  1	なし なし  下限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_16(agreeKind);
        }

        private static void TestCase1_X_16(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, new Hogera("No2"));
                Assert.AreEqual(1, association.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception); // OK
        }

        [TestMethod]
        public void TestCase1_1_17()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	17		なし  0..5	なし あり  下限 あり  OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_17(agreeKind);
        }

        private static void TestCase1_X_17(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            try
            {
                Association<Hogera> association = new Association<Hogera>(agreeKind, 5, null, false, true, "Name");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception); // OK
        }

        [TestMethod]
        public void TestCase1_1_18()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	1	18		なし  3	なし あり  下限-1	なし NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase1_X_18(agreeKind);
        }

        private static void TestCase1_X_18(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3"),
                new Hogera("No1")
            };
            bool exception = false;
            try
            {
                FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsTrue(exception); // NGならば、テストとしてはOK。
        }

        [TestMethod]
        public void TestCase1_2_1()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	1		集約  3..6	あり あり  下限 あり  NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_1(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_2()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	2		集約  0..5	なし なし  上限+1	なし NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_2(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_3()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	3		集約  3	なし あり  下限 なし  OK,3, YES
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_3(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_4()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	4		集約  3..6	あり なし  下限-1	なし OK,5, NO
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_4(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_5()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	5		集約  0..5	あり あり  上限 あり  NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_5(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_6()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	6		集約  3	なし なし  下限-1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_6(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_7()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	7		集約  3..6	なし なし  上限 なし  OK,6, NO
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_7(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_8()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	8		集約  3	なし あり  上限 なし  OK,3, YES
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_8(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_9()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	9		集約  0..1	なし あり  上限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_9(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_10()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	10		集約  1	なし なし  上限+1	なし NG
            // ありえない
        }

        [TestMethod]
        public void TestCase1_2_11()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	11		集約  1	なし なし  上限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_11(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_12()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	12		集約  3..6	あり あり  上限+1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_12(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_13()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	13		集約  0..1	あり なし  下限 なし  OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_13(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_14()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	14		集約  1	なし なし  下限-1	なし NG
            // ありえない
        }

        [TestMethod]
        public void TestCase1_2_15()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	15		集約  3	なし なし  上限+1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_15(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_16()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	16		集約  1	なし なし  下限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_16(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_17()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	17		集約  0..5	なし あり  下限 あり  OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_17(agreeKind);
        }

        [TestMethod]
        public void TestCase1_2_18()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	2	18		集約  3	なし あり  下限-1	なし NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase1_X_18(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_1()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	1		コンポジション 3..6	あり あり  下限 あり  NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_1(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_2()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	2		コンポジション 0..5	なし なし  上限+1	なし NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_2(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_3()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	3		コンポジション 3	なし あり  下限 なし  OK,3, YES
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_3(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_4()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	4		コンポジション 3..6	あり なし  下限-1	なし NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_4(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_5()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	5		コンポジション 0..5	あり あり  上限 あり  NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_5(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_6()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	6		コンポジション 3	なし なし  下限-1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_6(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_7()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	7		コンポジション 3..6	なし なし  上限 なし  OK,6, NO
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_7(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_8()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	8		コンポジション 3	なし あり  上限 なし  OK,3, YES
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_8(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_9()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	9		コンポジション 0..1	なし あり  上限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_9(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_10()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	10		コンポジション 1	なし なし  上限+1	なし NG
            // ありえない
        }

        [TestMethod]
        public void TestCase1_3_11()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	11		コンポジション 1	なし なし  上限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_11(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_12()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	12		コンポジション 3..6	あり あり  上限+1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_12(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_13()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	13		コンポジション 0..1	あり なし  下限 なし  OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_13(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_14()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	14		コンポジション 1	なし なし  下限-1	なし NG
            // ありえない
        }

        [TestMethod]
        public void TestCase1_3_15()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	15		コンポジション 3	なし なし  上限+1	あり NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_15(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_16()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	16		コンポジション 1	なし なし  下限 なし  OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_16(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_17()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	17		コンポジション 0..5	なし あり  下限 あり  OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_17(agreeKind);
        }

        [TestMethod]
        public void TestCase1_3_18()
        {
            // 集約種別 多重度 ユニーク オーダー    初期要素数 初期要素のインスタンス重複   期待結果(生成、残数、順序）
            // 1	3	18		コンポジション 3	なし あり  下限-1	なし NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase1_X_18(agreeKind);
        }

        ///
        /// 追加のテスト
        ///

        [TestMethod]
        public void TestCase2_1_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_1		なし	0..1	あり	あり	上限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_1(agreeKind);
        }

        private static void TestCase2_X_1(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, new Hogera("No1"));
            try
            {
                bool ans = association.Add(new Hogera("No2"));
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_2		なし	0..5	なし	なし	下限	1	OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_2(agreeKind);
        }

        private static void TestCase2_X_2(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 5, null, false, false);
            try
            {
                bool ans = association.Add(new Hogera("No1"));
                Assert.IsTrue(ans);
                Assert.AreEqual(1, association.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_3		なし	3..6	あり	なし	上限-1	0	OK,6,NO
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_3(agreeKind);
        }

        private static void TestCase2_X_3(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No5"),
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2"),
                new Hogera("No1")
            };
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, true, false);
            try
            {
                bool ans = association.Add(new Hogera("No1"));
                Assert.IsTrue(ans);
                Assert.AreEqual(6, association.Count);
                CheckNotOrdered( association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_4		なし	3..6	あり	あり	下限	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_4(agreeKind);
        }

        private static void TestCase2_X_4(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, true, true, "Name");
            try
            {
                bool ans = association.Add(obj);
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_5		なし	0..5	なし	あり	上限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_5(agreeKind);
        }

        private static void TestCase2_X_5(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No5"),
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2"),
                new Hogera("No1")
            };
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 5, list, false, true, "Name");
            try
            {
                bool ans = association.Add(new Hogera("No6"));
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_6()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_6		なし	0..5	あり	あり	上限-1	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_6(agreeKind);
        }

        private static void TestCase2_X_6(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 5, list, true, true, "Name");
            try
            {
                bool ans = association.Add(obj);
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_7()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_7		なし	0..1	なし	なし	下限	0	OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_7(agreeKind);
        }

        private static void TestCase2_X_7(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind);
            try
            {
                bool ans = association.Add(new Hogera("No2"));
                Assert.IsTrue(ans);
                Assert.AreEqual(1, association.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_8()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_8		なし	3..6	なし	なし	上限	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_8(agreeKind);
        }

        private static void TestCase2_X_8(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No6"),
                new Hogera("No5"),
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, false, false);
            try
            {
                bool ans = association.Add(obj);
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_9()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_9		なし	0..5	なし	なし	上限-1	1	OK,5,NO
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase2_X_9(agreeKind);
        }

        private static void TestCase2_X_9(AgreeKindEnum agreeKind,bool OK = false)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 5, list, false, false, "Name");
            try
            {
                bool ans = association.Add(obj);
                if (OK == false) {
                    Assert.IsTrue(ans);
                    Assert.AreEqual(5, association.Count);
                    CheckNotOrdered( association);
                } else { Assert.IsFalse(ans); }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase2_1_10()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_1_10		なし	0..1	なし	あり	下限	1	ありえない
        }
        [TestMethod]
        public void TestCase2_2_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_1		集約	0..1	あり	あり	上限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_1(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_2		集約	0..5	なし	なし	下限	1	OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_2(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_3		集約	3..6	あり	なし	上限-1	0	OK,6,NO
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_3(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_4		集約	3..6	あり	あり	下限	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_4(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_5		集約	0..5	なし	あり	上限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_5(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_6()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_6		集約	0..5	あり	あり	上限-1	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_6(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_7()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_7		集約	0..1	なし	なし	下限	0	OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_7(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_8()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_8		集約	3..6	なし	なし	上限	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_8(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_9()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_9		集約	0..5	なし	なし	上限-1	1	OK,5,NO
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase2_X_9(agreeKind);
        }
        [TestMethod]
        public void TestCase2_2_10()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_2_10		集約	0..1	なし	あり	下限	1	ありえない
        }
        [TestMethod]
        public void TestCase2_3_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_1		コンポジション	0..1	あり	あり	上限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_1(agreeKind);
        }
        [TestMethod]
        public void TestCase2_3_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_2		コンポジション	0..5	なし	なし	下限	1	OK
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_2(agreeKind);
        }
        [TestMethod]
        public void TestCase2_3_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_3		コンポジション	3..6	あり	なし	上限-1	0	OK,6,NO
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_3(agreeKind);
        }
        [TestMethod]
        public void TestCase2_3_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_4		コンポジション	3..6	あり	あり	下限	1	NG、コンポジションエラー
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_4(agreeKind);
        }
        [TestMethod]
        public void TestCase2_3_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_5		コンポジション	0..5	なし	あり	上限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_5(agreeKind);
        }
        [TestMethod]
        public void TestCase2_3_6()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_6		コンポジション	0..5	あり	あり	上限-1	1	NG、コンポジションエラー
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_6(agreeKind);
        }
        [TestMethod]
        public void TestCase2_3_7()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_7		コンポジション	0..1	なし	なし	下限	0	OK,1
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_7(agreeKind);
        }
        [TestMethod]
        public void TestCase2_3_8()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_8		コンポジション	3..6	なし	なし	上限	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_8(agreeKind);
        }
        [TestMethod]
        public void TestCase2_3_9()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_9		コンポジション	0..5	なし	なし	上限-1	1	NG、コンポジションエラー
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase2_X_9(agreeKind,true);
        }
        [TestMethod]
        public void TestCase2_3_10()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	追加対象含有数	期待結果(追加、残数、順序）
            // 2_3_10		コンポジション	0..1	なし	あり	下限	1	ありえない
        }

        ///
        /// 削除
        /// 
        [TestMethod]
        public void TestCase3_1_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_1		なし	0..1	なし	あり	上限	0	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_1(agreeKind);
        }

        private static void TestCase3_X_1(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            Hogera added = new Hogera("No1");
            Association<Hogera> association = new Association<Hogera>(agreeKind, added);
            try
            {
                bool ans = association.Remove(added);
                Assert.IsTrue(ans);
                Assert.AreEqual(0, association.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_2		なし	3..6	なし	なし	下限	2	OK
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_2(agreeKind);
        }

        private static void TestCase3_X_2(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, false, false);
            try
            {
                bool ans = association.Remove(obj);
                Assert.AreEqual(3, association.Count);
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_3		なし	0..5	あり	あり	下限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_3(agreeKind);
        }

        private static void TestCase3_X_3(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 5, null, true, true, "Name");
            try
            {
                bool ans = association.Remove(new Hogera("No1"));
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_4		なし	3..6	なし	あり	下限+1	2	一つ減らす、OK,3
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_4(agreeKind);
        }

        private static void TestCase3_X_4(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, false, true, "Name");
            try
            {
                bool ans = association.Remove(obj);
                Assert.AreEqual(3, association.Count);
                Assert.IsTrue(ans);
                CheckOrdered(association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_5		なし	3..6	あり	なし	上限	1	OK,5
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_5(agreeKind);
        }

        private static void TestCase3_X_5(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No6"),
                new Hogera("No5"),
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, true, false, "Name");
            try
            {
                bool ans = association.Remove(obj);
                Assert.AreEqual(5, association.Count);
                Assert.IsTrue(ans);
                CheckNotOrdered( association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_6()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_6		なし	0..5	なし	なし	下限+1	1	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_6(agreeKind);
        }

        private static void TestCase3_X_6(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>();
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 5, list, true, true, "Name");
            try
            {
                bool ans = association.Remove(obj);
                Assert.AreEqual(0, association.Count);
                Assert.IsTrue(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_7()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_7		なし	0..5	なし	なし	上限	2	一つ減らす、OK,4
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_7(agreeKind);
        }

        private static void TestCase3_X_7(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 5, list, false, false, "Name");
            try
            {
                bool ans = association.Remove(obj);
                Assert.AreEqual(4, association.Count);
                Assert.IsTrue(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_8()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_8		なし	3..6	あり	なし	下限+1	0	OK,3
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_8(agreeKind);
        }

        private static void TestCase3_X_8(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No4"),
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, true, false, "Name");
            try
            {
                bool ans = association.Remove(obj);
                Assert.AreEqual(3, association.Count);
                Assert.IsTrue(ans);
                CheckNotOrdered(association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_9()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_9		なし	3..6	あり	あり	下限	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_9(agreeKind);
        }

        private static void TestCase3_X_9(AgreeKindEnum agreeKind)
        {
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3"),
                new Hogera("No2")
            };
            Hogera obj = new Hogera("No1");
            list.Add(obj);
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind, 3, 6, list, true, true, "Name");
            try
            {
                bool ans = association.Remove(obj);
                Assert.AreEqual(3, association.Count);
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_10()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_10		なし	0..1	あり	なし	下限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_10(agreeKind);
        }

        private static void TestCase3_X_10(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            Association<Hogera> association = new Association<Hogera>(agreeKind);
            try
            {
                Hogera remove = new Hogera("No.3");
                bool ans = association.Remove(remove);
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_1_11()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_1_11		なし	0..1	あり	なし	上限	1	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase3_X_11(agreeKind);
        }

        private static void TestCase3_X_11(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            Hogera added = new Hogera("No1");
            Association<Hogera> association = new Association<Hogera>(agreeKind, added);
            try
            {
                bool ans = association.Remove(added);
                Assert.IsTrue(ans);
                Assert.AreEqual(0, association.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase3_2_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_1		集約	0..1	なし	あり	上限	0	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_1(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_2		集約	3..6	なし	なし	下限	2	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_2(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_3		集約	0..5	あり	あり	下限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_3(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_4		集約	3..6	なし	あり	下限+1	2	一つ減らす、OK,3
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_4(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_5		集約	3..6	あり	なし	上限	1	OK,5
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_5(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_6()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_6		集約	0..5	なし	なし	下限+1	1	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_6(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_7()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_7		集約	0..5	なし	なし	上限	2	一つ減らす、OK,4
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_7(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_8()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_8		集約	3..6	あり	なし	下限+1	0	OK,3
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_8(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_9()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_9		集約	3..6	あり	あり	下限	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_9(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_10()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_10		集約	0..1	あり	なし	下限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_10(agreeKind);
        }
        [TestMethod]
        public void TestCase3_2_11()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_2_11		集約	0..1	あり	なし	上限	1	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase3_X_11(agreeKind);
        }
        [TestMethod]
        public void TestCase3_3_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_1		コンポジション	0..1	なし	あり	上限	0	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase3_X_1(agreeKind);
        }
        [TestMethod]
        public void TestCase3_3_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_2		コンポジション	3..6	なし	なし	下限	2	NG
            bool exception = false;
            try
            {
                AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
                TestCase3_X_2(agreeKind);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            // コンポジションは、1つのインスタンスが2つ以上登録されると異常になるので、Exceptionが発生することを確認
            Assert.IsTrue(exception);
        }
        [TestMethod]
        public void TestCase3_3_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_3		コンポジション	0..5	あり	あり	下限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase3_X_3(agreeKind);
        }
        [TestMethod]
        public void TestCase3_3_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_4		コンポジション	3..6	なし	あり	下限+1	1	OK,3
            bool exception = false;
            try
            {
                AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
                TestCase3_X_4(agreeKind);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            // コンポジションは、1つのインスタンスが2つ以上登録されると異常になるので、Exceptionが発生することを確認
            Assert.IsTrue(exception);
        }
        [TestMethod]
        public void TestCase3_3_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_5		コンポジション	3..6	あり	なし	上限	1	OK,5
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase3_X_5(agreeKind);
        }
        [TestMethod]
        public void TestCase3_3_6()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_6		コンポジション	0..5	なし	なし	下限+1	1	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase3_X_6(agreeKind);
        }
        [TestMethod]
        public void TestCase3_3_7()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_7		コンポジション	0..5	なし	なし	上限	1	OK,4
            bool exception = false;
            try
            {
                AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
                TestCase3_X_7(agreeKind);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            // コンポジションは、1つのインスタンスが2つ以上登録されると異常になるので、Exceptionが発生することを確認
            Assert.IsTrue(exception);
        }
        [TestMethod]
        public void TestCase3_3_8()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_8		コンポジション	3..6	あり	なし	下限+1	0	OK,3
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase3_X_8(agreeKind);
        }
        [TestMethod]
        public void TestCase3_3_9()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_9		コンポジション	3..6	あり	あり	下限	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase3_X_9(agreeKind);
        }
        [TestMethod]
        public void TestCase3_3_10()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            // 3_3_10		コンポジション	0..1	あり	なし	下限	0	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase3_X_10(agreeKind);
        }
        [TestMethod]
        public void TestCase3_3_11()
        {
            // 集約種別	多重度	ユニーク	オーダー	現在要素数	削除対象含有数	期待結果（削除、残数）
            //3_3_11		コンポジション	0..1	あり	なし	上限	1	OK,0
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase3_X_11(agreeKind);
        }

        ///
        /// 交換
        ///
        [TestMethod]
        public void TestCase4_1_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_1_1		なし	3	あり	あり	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase4_X_1(agreeKind);
        }

        private static void TestCase4_X_1(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3")
            };
            Hogera oldhogera = new Hogera("No4");
            list.Add(oldhogera);
            Hogera addedhogera = new Hogera("No1");
            list.Add(addedhogera);
            FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, true, true, "Name");
            try
            {
                bool ans = association.Change(oldhogera, addedhogera);
                Assert.IsFalse(ans);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase4_1_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_1_2		なし	1	なし	なし	1	OK,1,
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase4_X_2(agreeKind);
        }

        private static void TestCase4_X_2(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            Hogera oldhogera = new Hogera("No4");
            FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, oldhogera);
            try
            {
                Hogera addedhogera = new Hogera("No1");
                bool ans = association.Change( oldhogera, addedhogera);
                Assert.IsTrue(ans);
                Assert.AreEqual(1, association.Count);
                Assert.AreEqual<Hogera>(association.End, addedhogera);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase4_1_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_1_3		なし	3	なし	なし	2	OK,3,NO
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase4_X_3(agreeKind);
        }

        private static void TestCase4_X_3(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No2")
            };
            Hogera oldhogera = new Hogera("No3");
            list.Add(oldhogera);
            list.Add(oldhogera);
            FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, false, "Name");
            try
            {
                Hogera addedhogera = new Hogera("No1");
                bool ans = association.Change(oldhogera, addedhogera);
                Assert.IsTrue(ans);
                Assert.AreEqual(3, association.Count);
                CheckNotOrdered(association); // 何か他のチェック方法にする
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase4_1_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_1_4		なし	1	あり	なし	0	OK,1,
            
            // ユニークやオーダーがないので、4_1_2と同じ
        }
        [TestMethod]
        public void TestCase4_1_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_1_5		なし	3	なし	あり	2	OK,3,YES
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase4_X_5(agreeKind);
        }

        private static void TestCase4_X_5(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No2")
            };
            Hogera oldhogera = new Hogera("No1");
            list.Add(oldhogera);
            list.Add(oldhogera);
            FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, true, "Name");
            try
            {
                //Hogera addedhogera = new Hogera("No1");
                bool ans = association.Change(oldhogera, oldhogera);
                Assert.IsTrue(ans);
                Assert.AreEqual(3, association.Count);
                CheckOrdered(association);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase4_1_6()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_1_6		なし	3	なし	あり	0	OK,3,YES
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            TestCase4_X_6(agreeKind);
        }

        private static void TestCase4_X_6(AgreeKindEnum agreeKind)
        {
            bool exception = false;
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No2")
            };
            Hogera oldhogera = new Hogera("No3");
            list.Add(oldhogera);
            list.Add(new Hogera("No4"));
            FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, true, "Name");
            try
            {
                Hogera addedhogera = new Hogera("No1");
                bool ans = association.Change(oldhogera, addedhogera);
                Assert.IsTrue(ans);
                Assert.AreEqual(3, association.Count);
                CheckOrdered(association); // 何か他のチェック方法にする
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                exception = true;
            }
            Assert.IsFalse(exception);
        }

        [TestMethod]
        public void TestCase4_1_7()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_1_7		なし	1	あり	あり	0	OK,1,

            // ユニークやオーダーがないので、4_1_2と同じ
        }
        [TestMethod]
        public void TestCase4_2_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_2_1		集約	3	あり	あり	1	NG
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase4_X_1(agreeKind);
        }
        [TestMethod]
        public void TestCase4_2_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_2_2		集約	1	なし	なし	1	OK,1,
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase4_X_2(agreeKind);
        }
        [TestMethod]
        public void TestCase4_2_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_2_3		集約	3	なし	なし	2	OK,3,NO
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase4_X_3(agreeKind);
        }
        [TestMethod]
        public void TestCase4_2_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_2_4		集約	1	あり	なし	0	OK,1,

            // ユニークやオーダーがないので、4_2_2と同じ
        }
        [TestMethod]
        public void TestCase4_2_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_2_5		集約	3	なし	あり	2	OK,3,YES
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase4_X_5(agreeKind);
        }
        [TestMethod]
        public void TestCase4_2_6()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_2_6		集約	3	なし	あり	0	OK,3,YES
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            TestCase4_X_6(agreeKind);
        }
        [TestMethod]
        public void TestCase4_2_7()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_2_7		集約	1	あり	あり	0	OK,1,
            
            // なし
        }
        [TestMethod]
        public void TestCase4_3_1()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_3_1		コンポジション	3	あり	あり	1	NG,コンポジションエラー
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase4_X_1(agreeKind);
        }
        [TestMethod]
        public void TestCase4_3_2()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_3_2		コンポジション	1	なし	なし	1	OK,1,
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase4_X_2(agreeKind);
        }
        [TestMethod]
        public void TestCase4_3_3()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_3_3		コンポジション	1	あり	なし	0	OK,1,
            
            // なし
        }
        [TestMethod]
        public void TestCase4_3_4()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_3_4		コンポジション	3	なし	あり	0	OK,3,YES
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            TestCase4_X_6(agreeKind);
        }
        [TestMethod]
        public void TestCase4_3_5()
        {
            // 集約種別	多重度	ユニーク	オーダー	追加対象含有数	期待結果（削除、残数、順序）
            // 4_3_5		コンポジション	1	あり	あり	0	OK,1,

            // なし
        }


        ///
        /// オーダー
        /// 
        [TestMethod]
        public void TestCase5_1_1()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_1_1		なし	3	あり	sbyte
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            List<sbyte> list = new List<sbyte>
            {
                3,
                1,
                2
            };
            FixedAssociation<sbyte> association = new FixedAssociation<sbyte>(agreeKind, 3, list, false, true);
            sbyte preObj = 0;
            foreach (sbyte obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_1_2()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_1_2		なし	0..3	あり	byte
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            List<byte> list = new List<byte>
            {
                3,
                1,
                2
            };
            FixedAssociation<byte> association = new FixedAssociation<byte>(agreeKind, 3, list, false, true);
            byte preObj = 0;
            foreach (byte obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_1_3()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_1_3		なし	3	あり	short	
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            List<short> list = new List<short>
            {
                3,
                1,
                2
            };
            FixedAssociation<short> association = new FixedAssociation<short>(agreeKind, 3, list, false, true);
            short preObj = 0;
            foreach (short obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_1_4()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_1_4		なし	0..3	あり	ushort	
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            List<ushort> list = new List<ushort>
            {
                3,
                1,
                2
            };
            FixedAssociation<ushort> association = new FixedAssociation<ushort>(agreeKind, 3, list, false, true);
            ushort preObj = 0;
            foreach (ushort obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_1_5()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_1_5		なし	3	あり	int	
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            List<int> list = new List<int>
            {
                -3,
                1,
                2
            };
            FixedAssociation<int> association = new FixedAssociation<int>(agreeKind, 3, list, false, true);
            int preObj = 0;
            foreach (int obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_1_6()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_1_6		なし	0..3	あり	uint	
            AgreeKindEnum agreeKind = AgreeKindEnum.None;
            List<uint> list = new List<uint>
            {
                3,
                1,
                2
            };
            FixedAssociation<uint> association = new FixedAssociation<uint>(agreeKind, 3, list, false, true);
            uint preObj = 0;
            foreach (uint obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_2_1()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_2_1		集約	3	あり	long
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            List<long> list = new List<long>
            {
                3,
                1,
                2
            };
            FixedAssociation<long> association = new FixedAssociation<long>(agreeKind, 3, list, false, true);
            long preObj = 0;
            foreach (long obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_2_2()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_2_2		集約	0..3	あり	char	
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            List<char> list = new List<char>
            {
                '3',
                '1',
                '2'
            };
            FixedAssociation<char> association = new FixedAssociation<char>(agreeKind, 3, list, false, true);
            char preObj = '0';
            foreach (char obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_2_3()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_2_3		集約	3	あり	float
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            List<float> list = new List<float>
            {
                0.3f,
                0.1f,
                0.2f
            };
            FixedAssociation<float> association = new FixedAssociation<float>(agreeKind, 3, list, false, true);
            float preObj = 0;
            foreach (float obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_2_4()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_2_4		集約	0..3	あり	double	
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            List<double> list = new List<double>
            {
                0.3,
                0.1,
                0.2
            };
            FixedAssociation<double> association = new FixedAssociation<double>(agreeKind, 3, list, false, true);
            double preObj = 0;
            foreach (double obj in association.Ends)
            {
                if (preObj != 0)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
            }
        }
        [TestMethod]
        public void TestCase5_2_5()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_2_5		集約	3	あり	bool	
            AgreeKindEnum agreeKind = AgreeKindEnum.Shared;
            List<bool> list = new List<bool>
            {
                true,
                false
            };
            FixedAssociation<bool> association = new FixedAssociation<bool>(agreeKind, 2, list, false, true);
            bool preObj = false;
            bool first = true;
            foreach (bool obj in association.Ends)
            {
                if (first == false)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
                first = false;
            }
        }
        [TestMethod]
        public void TestCase5_3_1()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_3_1		コンポジション	0..3	あり	decimal	
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            List<bool> list = new List<bool>
            {
                true,
                false
            };
            FixedAssociation<bool> association = new FixedAssociation<bool>(agreeKind, 2, list, false, true);
            bool preObj = false;
            bool first = true;
            foreach (bool obj in association.Ends)
            {
                if (first == false)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
                first = false;
            }
        }
        [TestMethod]
        public void TestCase5_3_2()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_3_2		コンポジション	3	あり	string	
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            List<string> list = new List<string>
            {
                "ABC",
                "Z",
                "A"
            };
            FixedAssociation<string> association = new FixedAssociation<string>(agreeKind, 3, list, false, true);
            string preObj = "";
            bool first = true;
            foreach (string obj in association.Ends)
            {
                if (first == false)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
                first = false;
            }
        }

        struct Piyo
        {
            public int No
            {
                set;get;
            }

            public Piyo(int no)
            {
                No = no;
            }
        }

        [TestMethod]
        public void TestCase5_3_3()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_3_3		コンポジション	0..3	あり	struct	
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            List<Piyo> list = new List<Piyo>
            {
                new Piyo(2),
                new Piyo(1),
                new Piyo(7)
            };
            FixedAssociation<Piyo> association = new FixedAssociation<Piyo>(agreeKind, 3, list, false, true,"No");
            Piyo preObj = new Piyo(0);
            bool first = true;
            foreach (Piyo obj in association.Ends)
            {
                if (first == false)
                {
                    //if (preObj.CompareTo(obj) > 0)
                    //{
                    //    Assert.AreEqual(obj, preObj);
                    //}
                }
                preObj = obj;
                first = false;
            }
        }

        [TestMethod]
        public void TestCase5_3_4()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_3_4		コンポジション	3	あり	enum	
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            List<PiyoPiyo> list = new List<PiyoPiyo>
            {
                PiyoPiyo.PIYO_4,
                PiyoPiyo.PIYO_5,
                PiyoPiyo.PIYO_1
            };
            FixedAssociation<PiyoPiyo> association = new FixedAssociation<PiyoPiyo>(agreeKind, 3, list, false, true);
            PiyoPiyo preObj = PiyoPiyo.PIYO_2;
            bool first = true;
            foreach (PiyoPiyo obj in association.Ends)
            {
                if (first == false)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
                first = false;
            }
        }
        [TestMethod]
        public void TestCase5_3_5()
        {
            // 集約種別	多重度	オーダー	追加対象含有数	期待結果
            // 5_3_5		コンポジション	0..3	あり	クラス	
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            List<Hogera> list = new List<Hogera>
            {
                new Hogera("No3", PiyoPiyo.PIYO_3),
                new Hogera("No2", PiyoPiyo.PIYO_2),
                new Hogera("No1", PiyoPiyo.PIYO_1)
            };
            FixedAssociation<Hogera> association = new FixedAssociation<Hogera>(agreeKind, 3, list, false, true, "PiyoPiyoEnum");
            Hogera preObj = null;
            bool first = true;
            foreach (Hogera obj in association.Ends)
            {
                if (first == false)
                {
                    if (preObj.CompareTo(obj) > 0)
                    {
                        Assert.AreEqual(obj, preObj);
                    }
                }
                preObj = obj;
                first = false;
            }
        }

        /// <summary>
        /// キー1つの正常系の追加
        /// </summary>
        [TestMethod]
        public void TestCase6_1_1()
        {
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            Association<int, Hogera> association = new Association<int, Hogera>(agreeKind, 1, new Hogera("No1"));
            association.Add(2, new Hogera("No2"));
            foreach(KeyValuePair<int,Hogera> pair in association.AllEnd)
            {
                System.Diagnostics.Debug.WriteLine(pair.Key + "/" + pair.Value.Name);
            }
            Hogera hogera = association.End(1);
        }

        /// <summary>
        /// キー1つの異常系の追加 キーの重複
        /// </summary>
        [TestMethod]
        public void TestCase6_1_2()
        {
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            Association<int, Hogera> association = new Association<int, Hogera>(agreeKind, 1, new Hogera("No1"));
            association.Add(2, new Hogera("No2"));
            bool exception = false;
            try
            {
                association.Add(2, new Hogera("No2"));
            }
            catch(Exception e)
            {
                exception = true;
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            Assert.IsFalse(exception);
        }

        /// <summary>
        /// キー1つの異常系の取得 存在しないキー
        /// </summary>
        [TestMethod]
        public void TestCase6_1_3()
        {
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            Association<int, Hogera> association = new Association<int, Hogera>(agreeKind, 1, new Hogera("No1"));
            association.Add(2, new Hogera("No2"));
            foreach (KeyValuePair<int, Hogera> pair in association.AllEnd)
            {
                System.Diagnostics.Debug.WriteLine(pair.Key + "/" + pair.Value.Name);
            }
            Hogera hogera2 = null;
            try
            {
                hogera2 = association.End(3);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            Assert.IsNull(hogera2);
        }

        /// <summary>
        /// キー2つの異常系の取得 存在しないキー
        /// </summary>
        [TestMethod]
        public void TestCase6_1_4()
        {
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            Association<int,int, Hogera> association = new Association<int,int, Hogera>(agreeKind,1, 2, new Hogera("No1"));
            association.Add(1,3, new Hogera("No2"));
            foreach (KeyValuePair<Tuple<int,int>, Hogera> pair in association.AllEnd)
            {
                System.Diagnostics.Debug.WriteLine(pair.Key + "/" + pair.Value.Name);
            }
            Hogera hogera2 = null;
            try
            {
                hogera2 = association.End(2,3);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            Assert.IsNull(hogera2);
            
        }

        /// <summary>
        /// キー2つの正常系の追加と削除
        /// </summary>
        [TestMethod]
        public void TestCase6_1_5()
        {
            AgreeKindEnum agreeKind = AgreeKindEnum.Composite;
            Association<int, int, Hogera> association = new Association<int, int, Hogera>(agreeKind, 1, 2, new Hogera("No1"));
            association.Add(1, 3, new Hogera("No2"));
            foreach (KeyValuePair<Tuple<int, int>, Hogera> pair in association.AllEnd)
            {
                System.Diagnostics.Debug.WriteLine(pair.Key + "/" + pair.Value.Name);
            }
            try
            {
                bool ans = association.Remove(1, 2);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            // Key1,3のデータは残っている
            int count = association.Count(1, 3);
            Assert.AreEqual(1, count);
            // Key1,2のデータは残っていない
            count = association.Count(1, 2);
            Assert.AreEqual(0, count);
        }
    }
}
