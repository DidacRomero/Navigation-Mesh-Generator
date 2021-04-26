using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularList<T> : List<T>//This class extends the functionality of a list to a Circular List
{
    public T GetNext(int i)
    {
        T ret;
        if(i == this.Count - 1)
            ret = this[0];
        else
            ret = this[i+1];

        return ret;
    }

    public T GetPrevious(int i)
    {
        T ret;
        if (i == 0)
            ret = this[this.Count - 1];
        else
            ret = this[i - 1];

        return ret;
    }

};
