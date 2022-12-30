using System.Collections;

using Mauve.Patterns;

namespace Freya
{
    internal class DependencyCollection : IDependencyCollection
    {
        public DependencyDescriptor this[int index] { get => null; set => _ = 3; }

        public int Count { get; }
        public bool IsReadOnly { get; }

        public void Add(DependencyDescriptor item) => throw new NotImplementedException();
        public IDependencyCollection AddScoped<T>() => throw new NotImplementedException();
        public IDependencyCollection AddScoped<T>(string alias) => throw new NotImplementedException();
        public IDependencyCollection AddScoped<T>(IFactory<T> factory) => throw new NotImplementedException();
        public IDependencyCollection AddScoped<T>(string alias, IFactory<T> factory) => throw new NotImplementedException();
        public IDependencyCollection AddScoped<T>(Func<T> factory) => throw new NotImplementedException();
        public IDependencyCollection AddScoped<T>(string alias, Func<T> factory) => throw new NotImplementedException();
        public IDependencyCollection AddSingleton<T>(T singleton) => throw new NotImplementedException();
        public IDependencyCollection AddSingleton(Type type, object instance) => throw new NotImplementedException();
        public IDependencyCollection AddTransient<T>() => throw new NotImplementedException();
        public IDependencyCollection AddTransient<T>(string alias) => throw new NotImplementedException();
        public IDependencyCollection AddTransient<T>(IFactory<T> factory) => throw new NotImplementedException();
        public IDependencyCollection AddTransient<T>(string alias, IFactory<T> factory) => throw new NotImplementedException();
        public IDependencyCollection AddTransient<T>(Func<T> factory) => throw new NotImplementedException();
        public IDependencyCollection AddTransient<T>(string alias, Func<T> factory) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        public bool Contains(DependencyDescriptor item) => throw new NotImplementedException();
        public void CopyTo(DependencyDescriptor[] array, int arrayIndex) => throw new NotImplementedException();
        public IEnumerator<DependencyDescriptor> GetEnumerator() => throw new NotImplementedException();
        public int IndexOf(DependencyDescriptor item) => throw new NotImplementedException();
        public void Insert(int index, DependencyDescriptor item) => throw new NotImplementedException();
        public bool Remove(DependencyDescriptor item) => throw new NotImplementedException();
        public void RemoveAt(int index) => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
