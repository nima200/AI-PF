using System.Collections.Generic;
/// <summary>
/// Custom class that creates a queue of given type T, capped at a certain amount of elements.
/// Anything enqueued to the queue beyond the limit, causes the first element of the queue to 
/// dequeue autmatically.
/// </summary>
/// <typeparam name="T">The type of the elements in the capped queue.</typeparam>
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
