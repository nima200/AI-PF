using System.Collections.Generic;
public class MyQueue<T>
{
    public Queue<T> Q = new Queue<T>();
    public int Limit { get; set; }

    public void Enqueue(T obj)
    {
        Q.Enqueue(obj);
        if (Q.Count > Limit)
        {
            Q.Dequeue();
        }
    }
}
