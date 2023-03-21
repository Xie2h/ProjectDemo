namespace Common
{
    /// <summary>
    /// distinct去重过滤器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisComparer<T> : IEqualityComparer<T>
    {
        private string _attribute;
        public DisComparer(string attribute)
        {
            _attribute = attribute;
        }
        public bool Equals(T? x, T? y)
        {
            dynamic dx = x;
            dynamic dy = y;
            var vx = dx.GetType().GetProperty(_attribute).GetValue(dx, null);
            var vy = dy.GetType().GetProperty(_attribute).GetValue(dy, null);
            return vx.Equals(vy);
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}