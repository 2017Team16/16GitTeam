using UnityEngine;
using System.Collections;

public enum BrotherState
{
    NONE = 0,
    NORMAL,     //通常時
    THROW,      //投げられている時
    WAIT,       //指示待ち    
    BACK        //兄の元へ戻っている時
}
