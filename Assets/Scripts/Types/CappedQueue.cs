using System.Collections.Generic;
public class CappedQueue<T>
{
    public Queue<T> Q = new Queue<T>();
    public int Limit { get; set; }

    public CappedQueue(int limit)
    {
        Limit = limit;
    }

    public void Enqueue(T obj)
    {
        Q.Enqueue(obj);
        if (Q.Count > Limit)
        {
            Q.Dequeue();
        }
    }

    public bool Contains(T obj)
    {
        return Q.Contains(obj);
    }
}
