namespace CodeLibrary.Core.DevToys
{
    /// <summary>
    /// Variable that can only be read once.
    /// </summary>
    public class ReadOnceVar<T> where T : struct
    {
        private T _Value;

        public T Value
        {
            get
            {
                T _retval = _Value;
                _Value = default;
                return _retval;
            }
            set
            {
                _Value = value;
            }
        }

        public ReadOnceVar(T value)
        {
            _Value = value;
        }

        public static implicit operator ReadOnceVar<T>(T value) => new ReadOnceVar<T>(value);

        public static implicit operator T(ReadOnceVar<T> value) => value.Value;
    }
}