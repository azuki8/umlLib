using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuki.UML
{
    /// <summary>
    /// コンポジションで全体に含まれている部分要素のリスト
    /// </summary>
    /// <remarks>すべての関連クラスで利用するためstaticになっている</remarks>
    internal class CompositPartList
    {
        /// <summary>
        /// 集約種別がコンポジションのとき、コンポジションの部分要素が他の全体要素に属しているかをチェックするためのリスト
        /// </summary>
        public static List<object> compsitList = new List<object>();
        /// <summary>
        /// コンポジションのチェックリストの排他用ロック
        /// </summary>
        public static object compsitLock = new object();
    }

    /// <summary>
    /// 集約種別 None:普通の関連、Shared:集約(白抜きひし形の関連)、Composite:コンポジション(黒塗りひし形の関連)
    /// </summary>
    public enum AgreeKindEnum
    {
        None,
        Shared,
        Composite,
    }

    /// <summary>
    /// 関連 多重度[n..m]
    /// </summary>
    /// <typeparam name="T">関連端の型</typeparam>
    public class Association<T>
    {
        // 集約種別
        private AgreeKindEnum agreeKind = AgreeKindEnum.None;

        // 関連端の実体リスト
        private List<T> roleList = null;

        // コレクションデータの編集フラグ
        private bool dirty = false;

        /// <summary>
        /// 関連端（関連の要素群）
        /// </summary>
        /// <remarks>関連の要素リストを取得できます</remarks>
        public IEnumerable<T> Ends
        {
            get
            {
                if( isOrdered == true)
                {
                    if (this.dirty == true)
                    {
                        roleList.Sort(CompareFunc);
                        this.dirty = false;
                    }
                }
                return roleList;
            }
        }

        /// <summary>
        /// 関連端（関連の要素）
        /// </summary>
        /// <remarks>多重度1に設定された関連で利用するプロパティ（多重度1までの関連ではイテレータを使わずにすむようにするため）。多重度2以上ではFailします。/remarks>
        public T End
        {
            get
            {
                if (maxSize > 1)
                {
                    System.Diagnostics.Debug.Fail("Association:多重度2以上の関連では、Endを利用できません");
                    return default(T);
                }
                if (this.Count == 1)
                {
                    return roleList.First<T>();
                } else if ( this.Count == 0)
                {
                    return default(T);
                } else
                {
                    System.Diagnostics.Debug.Fail("Association:ありえない");
                    return default(T);
                }
            }
        }

        /// <summary>
        /// 関連端の要素数
        /// </summary>
        public int Count
        {
            get
            {
                return roleList.Count;
            }
        }

        /// <summary>
        /// private C#で構造体かチェックする関数
        /// </summary>
        /// <returns>bool true=構造体 false=それ以外/returns>
        private bool IsStruct()
        {
            return (typeof(T).IsValueType && !typeof(T).IsEnum);
        }

        /// <summary>
        /// private ソートするための関数
        /// </summary>
        /// <remarks>実験して作成したため、バグがある可能性あり。注意</remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int CompareFunc(T a, T b)
        {
            if( typeof(T).IsPrimitive == true || typeof(T).Equals(typeof(string)) || typeof(T).IsEnum == true ) // stringはIsPrimitiveに非該当
            {
                if (a is IComparable comparable)
                {
                    return comparable.CompareTo(b);
                }
            }
            else if( typeof(T).IsClass == true || IsStruct() == true)
            {
                var ValueProperty = typeof(T).GetProperty(orderPropertyName);
                if( ValueProperty == null)
                {
                    throw new ArgumentException("Orderの属性を入力してください:コンストラクタでのチェックが抜けた？");
                }
                var aValue = ValueProperty.GetValue(a);
                var bValue = ValueProperty.GetValue(b);
                if (aValue is IComparable comparable)
                {
                    // int , string はここを通った
                    // double,int16,int32,DateTime など未テスト
                    return comparable.CompareTo(bValue);
                }

                if (aValue is IComparer comparer)
                {
                    return comparer.Compare(aValue, bValue);
                }
            }
            return 0;
        }

        private int minSize = 0;
        private int maxSize = 0;
        private bool isOrdered = false; // UML上デフォルトはfalse
        private bool isUnique = true; // UML上デフォルトはtrue
        private string orderPropertyName = "";

        /// <summary>
        /// 関連 0..m のコンストラクタ
        /// </summary>
        /// <param name="_agreeKind">集約種別</param>
        /// <param name="_maxSize">多重度 最大値m</param>
        /// <param name="_defaultList">m個以内の初期要素リスト</param>
        /// <param name="_isUnique">{unique}制約 true(あり) false(なし)</param>
        /// <param name="_isOrdered">{orderd}制約 true(あり) false(なし)</param>
        /// <param name="_orderPropertyName">orderedの順序を決めるプロパティ名</param>
        public Association(AgreeKindEnum _agreeKind, int _maxSize, List<T> _defaultList = null, bool _isUnique = true, bool _isOrdered = false, string _orderPropertyName = "")
        {
            agreeKind = _agreeKind;
            maxSize = _maxSize;
            isOrdered = _isOrdered;
            isUnique = _isUnique;
            orderPropertyName = _orderPropertyName;

            // 並び順があって
            if (isOrdered == true)
            {
                // int,byte,double,float... stringでなく、
                if (typeof(T).IsPrimitive == true || typeof(T).Equals(typeof(string)))
                {
                    // そのままの型であれば、
                    if(orderPropertyName != "")
                    {
                        throw new ArgumentException("クラス・構造体以外では、_orderPropertyNameを指定できません");
                    }
                } else
                { 
                    // クラスまたは、構造体ならば、
                    if (typeof(T).IsClass == true || IsStruct() == true )
                    {
                        // _orderPropertyNameが存在しなければ、
                        var ValueProperty = typeof(T).GetProperty(orderPropertyName);
                        if (ValueProperty == null)
                        {
                            throw new ArgumentException("_orderPropertyNameに指定されたプロパティがありません。フィールドの場合はプロパティに変更してください");
                        }
                    }
                }
            }

            roleList = new List<T>(maxSize); // 初期化時に最大サイズ取得しておく

            if (_defaultList != null)
            {
                if(maxSize < _defaultList.Count)
                {
                    throw new ArgumentException("初期リストが最大数より多い");
                }
                // 最小値以上最大値未満のリストで初期化する
                foreach (T item in _defaultList)
                {
                    if (Add(item) == false)
                    {
                        throw new ArgumentException("初期要素が不正です。コンストラクタを中断しました。");
                    }
                }
            }
        }

        /// <summary>
        /// 関連 n..m のコンストラクタ
        /// </summary>
        /// <param name="_agreeKind">集約種別</param>
        /// <param name="_minSize">多重度 最小値n</param>
        /// <param name="_maxSize">多重度 最大値m</param>
        /// <param name="_defaultList">n～m個の初期要素リスト</param>
        /// <param name="_isUnique">{unique}制約 true(あり) false(なし)</param>
        /// <param name="_isOrdered">{orderd}制約 true(あり) false(なし)</param>
        /// <param name="_orderPropertyName">orderedの順序を決めるプロパティ名</param>
        public Association(AgreeKindEnum _agreeKind, int _minSize, int _maxSize, List<T> _defaultList = null, bool _isUnique = true, bool _ordered = false, string _orderPropertyName = "") : this(_agreeKind, _maxSize, _defaultList, _isUnique, _ordered, _orderPropertyName)
        {
            if( _minSize == _maxSize)
            {
                throw new ArgumentException("多重度の最小値と最大値が同じ場合は、別のクラスを使用してください");
            }

            minSize = _minSize;
            if( minSize > 0)
            {
                if(_defaultList == null)
                {
                    throw new ArgumentException("初期リストが不足しています");
                } else if(minSize > _defaultList.Count)
                {
                    throw new ArgumentException("初期リストが最小数より少ない");
                }
            }
        }

        /// <summary>
        /// 関連 0..1 のコンストラクタ
        /// </summary>
        /// <remarks>初期要素0のコンストラクタ</remarks>
        /// <param name="_agreeKind">集約種別</param>
        public Association(AgreeKindEnum _agreeKind) : this(_agreeKind, 0, 1, null, false, false, "")
        {
            // 多重度[0..1]なので、
            // 最小値 0
            // 最大値 1
            // このコンストラクタは、初期設定要素なしなので、null
            // isUnique は、要素が1つまでで関係ないので、falseを設定
            // isOrdered は、要素が1つまでで関係ないので、falseを設定
            // orderPropertyName も、Orderが関係ないので、""を設定
        }

        /// <summary>
        /// 関連 0..1 のコンストラクタ
        /// </summary>
        /// <remarks>初期要素のあるコンストラクタ</remarks>
        /// <param name="_agreeKind">集約種別</param>
        /// <param name="_defaultValue">初期要素</param>
        public Association(AgreeKindEnum _agreeKind, T _defaultValue) : this(_agreeKind)
        {
            bool ans = this.Add(_defaultValue);
            System.Diagnostics.Debug.Assert(ans == true);
        }

        /// <summary>
        /// 指定要素を加える
        /// </summary>
        /// <param name="value">追加対象の要素</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Add(T value)
        {
            lock (CompositPartList.compsitLock)
            {
                // 重複チェック
                if (isUnique == true)
                {
                    if (roleList.Contains(value) == true)
                    {
                        System.Diagnostics.Debug.Fail("isUnique設定ですが、オブジェクトの重複があります");
                        return false;
                    }
                }

                // 最大値を超える場合は、NG
                if (roleList.Count >= maxSize)
                {
                    System.Diagnostics.Debug.Fail("Association最大値越え");
                    return false;
                }
                else
                {
                    // コンポジションの場合の親チェック
                    if (agreeKind == AgreeKindEnum.Composite)
                    {
                        if (CompositPartList.compsitList.Contains(value) == true)
                        {
                            System.Diagnostics.Debug.Fail("コンポジションの部分要素が複数の親にセットされます");
                            return false;
                        }
                    }

                    bool ans = false;
                    roleList.Add(value);
                    ans = true;
                    this.dirty = true;
                    // コンポジションの管理リストへの追加
                    if (agreeKind == AgreeKindEnum.Composite)
                    {
                        if (ans == true)
                        {
                            {
                                CompositPartList.compsitList.Add(value);
                            }
                        }
                    }
                    return ans;
                }
            }
        }

        /// <summary>
        /// 指定要素を取り除く
        /// </summary>
        /// <param name="value">除外対象の要素</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Remove(T value)
        {
            lock (CompositPartList.compsitLock)
            {
                if (roleList.Count <= minSize)
                {
                    System.Diagnostics.Debug.Fail("Association最小値越え");
                    return false;
                }
                else
                {
                    bool ans = false;
                    if (roleList.Contains(value) == false)
                    {
                        ans = false;
                    }
                    else
                    {
                        roleList.Remove(value);
                        ans = true;
                    }
                    // コンポジションの場合、管理リストから削除
                    if (agreeKind == AgreeKindEnum.Composite)
                    {
                        if (ans == true)
                        {
                            CompositPartList.compsitList.Remove(value);
                        }
                    }
                    return ans;
                }
            }
        }

        /// <summary>
        /// 要素の差し替え
        /// </summary>
        /// <param name="_old">差替前の要素</param>
        /// <param name="_new">差替後の要素</param>
        /// <remarks>コンポジションで他の全体に保持されている要素に差し替えようとすると失敗します</remarks>
        /// <returns>true:成功、false:失敗</returns>
        public bool Change(T _old, T _new)
        {
            // 交換する
            if (false == this.Remove(_old))
            {
                return false;
            }
            if (false == this.Add(_new))
            {
                this.Add(_old);
                System.Diagnostics.Debug.Fail("交換に失敗しました");
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 関連 限定子付き 多重度[0..1]
    /// </summary>
    /// <remarks>限定子1つ。限定子に対して多重度[0..1]</remarks>
    /// <typeparam name="TKey">関連端の限定子の型</typeparam>
    /// <typeparam name="TValue">関連端の型</typeparam>
    public class Association<TKey, TValue> 
    {
        // 集約種別
        private AgreeKindEnum agreeKind = AgreeKindEnum.None;
        // 関連 n..m
        private Dictionary<TKey, TValue> roleList = new Dictionary<TKey, TValue>();

        /// <summary>
        /// 全ての関連端
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, TValue>> AllEnd
        {
            get
            {
                return roleList;
            }
        }

        /// <summary>
        /// 指定された限定子の関連端
        /// </summary>
        /// <param name="key">限定子の値</param>
        /// <returns>指定された限定子の関連端</returns>
        public TValue End(TKey key)
        {
            if( roleList.ContainsKey(key) == false)
            {
                System.Diagnostics.Debug.Fail("存在しないキー(" + key + ")です");
                return default(TValue);
            }
            return roleList[key];
        }

        /// <summary>
        /// 指定された限定子の関連端の要素数
        /// </summary>
        /// <param name="key">限定子の値</param>
        /// <returns>指定された限定子の関連端の要素数</returns>
        public int Count(TKey key)
        {
            if (roleList.ContainsKey(key) == false)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 関連 限定子付き 0..1 のコンストラクタ
        /// </summary>
        /// <param name="_agreeKind">集約種別</param>
        public Association(AgreeKindEnum _agreeKind)
        {
            if (typeof(TValue).IsPrimitive == true || typeof(TValue).Equals(typeof(string)))
            {
                System.Diagnostics.Debug.Fail(typeof(TValue).Name + "は、限定子付きの関連では使えません。プロパティを持つクラスか構造体のみ対応します");
            }
            agreeKind = _agreeKind;
        }

        /// <summary>
        /// 関連 限定子付き 0..1 のコンストラクタ
        /// </summary>
        /// <param name="_agreeKind">集約種別</param>
        /// <param name="_key">初期要素の限定子の値</param>
        /// <param name="_value">初期要素</param>
        public Association(AgreeKindEnum _agreeKind, TKey _key, TValue _value) : this(_agreeKind)
        {
            bool ans = this.Add(_key, _value);
            System.Diagnostics.Debug.Assert(ans == true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">追加対象の要素</param>
        /// <returns>true:成功、false:失敗</returns>

        /// <summary>
        /// 指定要素を加える
        /// </summary>
        /// <param name="key">追加対象の限定子の値</param>
        /// <param name="value">追加要素</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Add(TKey key, TValue value)
        {
            lock (CompositPartList.compsitLock)
            {
                // コンポジションの場合の親チェック
                if (agreeKind == AgreeKindEnum.Composite)
                {
                    if (CompositPartList.compsitList.Contains(value) == true)
                    {
                        System.Diagnostics.Debug.Fail("コンポジションの部分要素が複数の親にセットされます");
                        return false;
                    }
                }

                // キーの重複チェック
                if (roleList.ContainsKey(key) == true)
                {
                    // キーに重複があります
                    System.Diagnostics.Debug.Fail("キーの重複です。多重度0..1のため、重複できません。");
                    return false;
                }

                bool ans = true;
                try
                {
                    roleList.Add(key, value);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Fail("不具合の可能性あり" + e.Message);
                    ans = false;
                }
                finally
                {

                }

                // コンポジションの管理リストへの追加
                if (agreeKind == AgreeKindEnum.Composite)
                {
                    if (ans == true)
                    {
                        {
                            CompositPartList.compsitList.Add(value);
                        }
                    }
                }
                return ans;
            }
        }

        /// <summary>
        /// 指定要素を取り除く
        /// </summary>
        /// <param name="key">除外対象の限定子の値</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Remove(TKey key)
        {
            lock (CompositPartList.compsitLock)
            {
                bool ans;
                if (roleList.ContainsKey(key) == false)
                {
                    // 含まれていないものを取り除こうとしたので、false
                    return false;
                }
                else
                {
                    TValue value = roleList[key];
                    ans = roleList.Remove(key);
                    // コンポジションの場合、管理リストから削除
                    if (agreeKind == AgreeKindEnum.Composite)
                    {
                        if (ans == true)
                        {
                            CompositPartList.compsitList.Remove(value);
                        }
                    }
                    return ans;
                }
            }
        }

        /// <summary>
        /// 変更
        /// </summary>
        /// <param name="_key">差替対象の限定子</param>
        /// <param name="_new">差替後の要素</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Change(TKey _key, TValue _new)
        {
            TValue _old = roleList[_key];

            // 交換する
            if (false == this.Remove(_key))
            {
                return false;
            }
            if (false == this.Add(_key, _new))
            {
                this.Add(_key, _old);
                System.Diagnostics.Debug.Fail("交換に失敗しました");
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 関連 限定子付き 多重度[0..1]
    /// </summary>
    /// <remarks>限定子2つ。2つの限定子の組合せに対して多重度[0..1]</remarks>
    /// <typeparam name="TKey1">関連端の限定子1の型</typeparam>
    /// <typeparam name="TKey2">関連端の限定子2の型</typeparam>
    /// <typeparam name="TValue">関連端の型</typeparam>
    public class Association<TKey1, TKey2, TValue> where TValue : class // where T : class が Tは参照型のみの意味
    {
        private Association<Tuple<TKey1, TKey2>, TValue> _multiAccociation = null;

        /// <summary>
        /// 関連 2限定子付き 0..1 のコンストラクタ
        /// </summary>
        /// <param name="_agreeKind">集約種別</param>
        public Association(AgreeKindEnum _agreeKind)
        {
            _multiAccociation = new Association<Tuple<TKey1, TKey2>, TValue>(_agreeKind);
        }

        /// <summary>
        /// 関連 限定子付き 0..1 のコンストラクタ
        /// </summary>
        /// <param name="_agreeKind">集約種別</param>
        /// <param name="_key">初期要素の限定子の値</param>
        /// <typeparam name="TKey1">初期要素の限定子1の値</typeparam>
        /// <typeparam name="TKey2">初期要素の限定子2の値</typeparam>
        /// <param name="_value">初期要素</param>
        public Association(AgreeKindEnum _agreeKind, TKey1 _key1, TKey2 _key2, TValue _value) : this(_agreeKind)
        {
            bool ans = _multiAccociation.Add(Tuple.Create<TKey1, TKey2>(_key1,_key2),_value);
            System.Diagnostics.Debug.Assert(ans == true);
        }

        /// <summary>
        /// 全ての関連端
        /// </summary>
        public IEnumerable<KeyValuePair<Tuple<TKey1, TKey2>, TValue>> AllEnd
        {
            get
            {
                return _multiAccociation.AllEnd;
            }
        }

        /// <summary>
        /// 指定された限定子の関連端
        /// </summary>
        /// <typeparam name="TKey1">限定子1の値</typeparam>
        /// <typeparam name="TKey2">限定子2の値</typeparam>
        /// <returns>指定された限定子の関連端</returns>
        public TValue End(TKey1 _key1,TKey2 _key2)
        {
            Tuple<TKey1, TKey2> keyset = new Tuple<TKey1, TKey2>(_key1, _key2);
            return _multiAccociation.End(keyset);
        }

        /// <summary>
        /// 指定された限定子の関連端の要素数
        /// </summary>
        /// <typeparam name="TKey1">限定子1の値</typeparam>
        /// <typeparam name="TKey2">限定子2の値</typeparam>
        /// <returns>指定された限定子の関連端の要素数</returns>
        public int Count(TKey1 _key1, TKey2 _key2)
        {
            Tuple<TKey1, TKey2> keyset = new Tuple<TKey1, TKey2>(_key1, _key2);
            return _multiAccociation.Count(keyset);
        }

        /// <summary>
        /// 指定要素を加える
        /// </summary>
        /// <param name="key1">追加対象の限定子1の値</param>
        /// <param name="key2">追加対象の限定子2の値</param>
        /// <param name="value">追加要素</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Add(TKey1 key1, TKey2 key2, TValue value)
        {
            return _multiAccociation.Add(Tuple.Create<TKey1, TKey2>(key1, key2), value);
        }

        /// <summary>
        /// 指定要素を取り除く
        /// </summary>
        /// <param name="key1">除外対象の限定子1の値</param>
        /// <param name="key2">除外対象の限定子2の値</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Remove(TKey1 key1, TKey2 key2)
        {
            return _multiAccociation.Remove(Tuple.Create<TKey1, TKey2>(key1, key2));
        }

        /// <summary>
        /// 変更
        /// </summary>
        /// <param name="key1">差替対象の限定子1の値</param>
        /// <param name="key2">差替対象の限定子2の値</param>
        /// <param name="_new">差替後の要素</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Change(TKey1 key1, TKey2 key2, TValue _new)
        {
            return _multiAccociation.Change(Tuple.Create<TKey1, TKey2>(key1, key2),_new);
        }
    }

    /// <summary>
    /// 関連 多重度[n]
    /// </summary>
    /// <typeparam name="T">関連端の型</typeparam>
    public class FixedAssociation<T> 
    {
        private Association<T> association = null;
        private int fixedSize = 0;

        /// <summary>
        /// 関連 多重度n のコンストラクタ
        /// </summary>
        /// <param name="_agreeKind">集約種別</param>
        /// <param name="_no">多重度[n]</param>
        /// <param name="_defaultValue">n個の初期要素リスト</param>
        /// <param name="_isUnique">{unique}制約 true(あり) false(なし)</param>
        /// <param name="_isOrdered">{orderd}制約 true(あり) false(なし)</param>
        /// <param name="_orderPropertyName">orderedの順序を決めるプロパティ名</param>
        public FixedAssociation(AgreeKindEnum _agreeKind, int _no, List<T> _defaultValue, bool _isUnique = true, bool _isOrdered = false, string _orderPropertyName = "")
        {
            if (_defaultValue.Count != _no)
            {
                throw new ArgumentException("初期設定要素数不一致(" + _defaultValue.Count + "/" + _no + ")");
            }
            association = new Association<T>(_agreeKind, 0, _no + 1, _defaultValue, _isUnique, _isOrdered, _orderPropertyName);
            fixedSize = _no;
        }

        /// <summary>
        /// 関連 多重度1 のコンストラクタ
        /// </summary>
        /// <param name="_agreeKind">集約種別</param>
        /// <param name="value">初期要素</param>
        public FixedAssociation(AgreeKindEnum _agreeKind, T value) : this(_agreeKind,1, new List<T>() { value }, false,false,"")
        {
            
        }

        /// <summary>
        /// 関連端（関連の要素群）
        /// </summary>
        /// <remarks>関連の要素リストを取得できます</remarks>
        public IEnumerable<T> Ends
        {
            get
            {
                return association.Ends;
            }
        }

        /// <summary>
        /// 関連端（関連の要素）
        /// </summary>
        /// <remarks>多重度1に設定された関連で利用するプロパティ（多重度1までの関連ではイテレータを使わずにすむようにするため）。多重度2以上ではFailします。/remarks>
        public T End
        {
            get
            {
                if (fixedSize > 1)
                {
                    System.Diagnostics.Debug.Fail("Association:多重度2以上の関連では、Endを利用できません");
                    return default(T);
                }
                if ( this.Count == 1)
                {
                    return association.Ends.First<T>();
                } else if( this.Count == 0)
                {
                    return default(T);
                } else
                {
                    System.Diagnostics.Debug.Fail("ありえない");
                    return default(T);
                }
            }
        }

        /// <summary>
        /// 関連端の要素数
        /// </summary>
        public int Count
        {
            get
            {
                return association.Count;
            }
        }

        /// <summary>
        /// 変更
        /// </summary>
        /// <param name="_old">差替対象の要素</param>
        /// <param name="_new">差替後の要素</param>
        /// <returns>true:成功、false:失敗</returns>
        public bool Change(T _old, T _new)
        {
            if (association.Ends.Contains(_old) == false)
            {
                // 古い方が含まれていない
                return false;
            }
            else
            {
                // 交換する
                if (false == association.Remove(_old))
                {
                    return false;
                }
                if (false == association.Add(_new))
                {
                    association.Add(_old);
                    System.Diagnostics.Debug.Fail("交換に失敗しました");
                    return false;
                }
                return true;
            }
        }
    }
}




