using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{

    public EnemyState currentState { get; private set; }// 当前状态

    // 初始化状态
    public void Initialize(EnemyState _StartState)
    {
        currentState = _StartState;
        currentState.Enter();
    }

    // 改变状态
    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
