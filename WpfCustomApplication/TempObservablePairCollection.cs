using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MVVMCore;

namespace TempWpfElements
{
    public class Pair<TKey, TValue> : CoreVM
    {
        protected TKey _key;
        protected TValue _value;

        public Pair()
        {

        }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public Pair(KeyValuePair<TKey, TValue> pair)
        {
            Key = pair.Key;
            Value = pair.Value;
        }

        public TKey Key
        {
            get { return _key; }
            set
            {
                if (
                    (_key == null && value != null)
                    || (_key != null && value == null)
                    || !_key.Equals(value))
                {
                    SetField(ref _key, value);
                }
            }
        }

        public TValue Value
        {
            get { return _value; }
            set
            {
                if (
                    (_value == null && value != null)
                    || (_value != null && value == null)
                    || (_value != null && !_value.Equals(value)))
                {
                    SetField(ref _value, value);
                }
            }
        }
    }

    public class ObservablePairCollection<TKey, TValue> : ObservableCollection<Pair<TKey, TValue>>, INotifyPropertyChanged //!! нужна ли ObservableCollection
        //where TKey : struct
        //where TValue : class
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public ObservablePairCollection() : base()
        {
        }

        public ObservablePairCollection(IEnumerable<Pair<TKey, TValue>> enumerable) : base(enumerable)
        {
        }

        public ObservablePairCollection(List<Pair<TKey, TValue>> list) : base(list)
        {
        }

        public ObservablePairCollection(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var kv in dictionary)
            {
                base.Add(new Pair<TKey, TValue>(kv));
            }
        }

        #region Операции над массивом
        public void Add(TKey key, TValue value)
        {
            this.Add(new Pair<TKey, TValue>(key, value));
        }

        public new void Add(Pair<TKey, TValue> pair)
        {
            base.Add(pair);

            OnPropertyChanged(nameof(Count));
            _values?.Changed(pair.Value);
            _keys?.Changed(pair.Key);
        }

        public void Remove(TKey key)
        {
            var pair = this.First(x => object.Equals(x.Key, key)); // object.Equals in reason any parameter may be null
            Remove(pair);
        }

        public void Remove(TValue value)
        {
            var pair = this.First(x => object.Equals(x.Value, value));
            Remove(pair);
        }
        
        public new void Remove(Pair<TKey, TValue> pair)
        {
            var index = this.IndexOf(pair);
            base.Remove(pair);

            OnPropertyChanged(nameof(Count));
            _values?.Changed(pair, index);
            _keys?.Changed(pair, index);
        }

        #endregion

        private ObservablePairCollection<TKey, TValue>.PairCollectionValues _values;
        public ObservablePairCollection<TKey, TValue>.PairCollectionValues Values
        {
            get
            {
                if (_values == null)
                    _values = new ObservablePairCollection<TKey, TValue>.PairCollectionValues(this);
                return this._values;
            }
        }

        private ObservablePairCollection<TKey, TValue>.PairCollectionKeys _keys;
        public ObservablePairCollection<TKey, TValue>.PairCollectionKeys Keys
        {
            get
            {
                if (_keys == null)
                    _keys = new ObservablePairCollection<TKey, TValue>.PairCollectionKeys(this);
                return this._keys;
            }
        }

        #region Values property realisation
        public sealed class PairCollectionValues : IEnumerable<TValue>, INotifyCollectionChanged
        {
            //inteface INotifyCollectionChanged
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private ObservablePairCollection<TKey, TValue> _dictionary;

            public PairCollectionValues(ObservablePairCollection<TKey, TValue> dictionary)
            {
                this._dictionary = dictionary;
            }

            public void Changed(TValue item)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }

            public void Changed(Pair<TKey, TValue> pair, int index)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, pair.Value, index));
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return new Enumerator(_dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class Enumerator : IEnumerator<TValue>
            {
                private ObservablePairCollection<TKey, TValue> _dictionary;
                private TValue _currentValue = default(TValue);
                private int index = 0;
                //!!private int version;

                public Enumerator(ObservablePairCollection<TKey, TValue> dictionary)
                {
                    this._dictionary = dictionary;
                    //this.version = dictionary.version;
                }

                public TValue Current => _currentValue;

                object IEnumerator.Current => _currentValue;

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    // this.version != _dictionary.Version
                    //for (;this.index < this._dictionary.Count; this.index++)
                    if (this.index < this._dictionary.Count)
                    {
                        this._currentValue = this._dictionary[this.index].Value;
                        this.index++;
                        return true;
                    }
                    this.index = this._dictionary.Count + 1;
                    this._currentValue = default(TValue);
                    return false;
                }

                public void Reset()
                {
                    //throw
                    this.index = 0;
                    this._currentValue = default(TValue);
                }
            }
        }

        #endregion

        #region Keys property realisation
        public sealed class PairCollectionKeys : IEnumerable<TKey>, INotifyCollectionChanged
        {
            //interface INotifyCollectionChanged
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private ObservablePairCollection<TKey, TValue> _dictionary;

            public PairCollectionKeys(ObservablePairCollection<TKey, TValue> dictionary)
            {
                this._dictionary = dictionary;
            }

            public void Changed(TKey key)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key));
            }

            public void Changed(Pair<TKey,TValue> pair, int index)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, pair.Key, index));
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                return new Enumerator(_dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private struct Enumerator : IEnumerator<TKey>
            {
                private ObservablePairCollection<TKey, TValue> _dictionary;
                private TKey _currentValue = default(TKey);
                private int index = 0;
                //!! private int version;

                public Enumerator(ObservablePairCollection<TKey, TValue> dictionary)
                {
                    this._dictionary = dictionary;
                    //this.version = dictionary.varsion;
                }

                public TKey Current => _currentValue;

                object IEnumerator.Current => _currentValue;

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    if (this.index < this._dictionary.Count)
                    {
                        this._currentValue = this._dictionary[this.index].Key;
                        this.index++;
                        return true;
                    }
                    this.index = this._dictionary.Count + 1;
                    this._currentValue = default(TKey);
                    return false;
                }

                public void Reset()
                {
                    this.index = 0;
                    this._currentValue = default(TKey);
                }
            }
        }

        #endregion
    }
}
