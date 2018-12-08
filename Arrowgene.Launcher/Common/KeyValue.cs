namespace Arrowgene.Launcher.Common
{
    public class KeyValue<K, V>
    {
        public K Key { get; set; }

        public V Value { get; set; }

        public KeyValue()
        {

        }

        public KeyValue(K key, V value)
        {
            this.Key = key;
            this.Value = value;
        }

    }
}
