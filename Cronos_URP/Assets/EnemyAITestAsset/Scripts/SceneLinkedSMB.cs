using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// StateMachineBehaviour 를 상속받아 추가 기능성을 더한 클래스.
/// StateMucine을 가진 객체의 모노비헤이버를 불러올 수 있고,
/// Enter / Update /  Exit 이벤트 외에도 애니메이션 트랜지션에 대한 업데이트가 추가되었다.
/// </summary>
/// <typeparam name="TMonoBehaviour">MonoBehaviour를 상속받은 클래스</typeparam>
public class SceneLinkedSMB<TMonoBehaviour> : SealedSMB
        where TMonoBehaviour : MonoBehaviour
{
    protected TMonoBehaviour _monoBehaviour;

    bool _firstFrameHappened;
    bool _lastFrameHappened;

    public static void Initialise(Animator animator, TMonoBehaviour monoBehaviour)
    {
        SceneLinkedSMB<TMonoBehaviour>[] sceneLinkedSMBs = animator.GetBehaviours<SceneLinkedSMB<TMonoBehaviour>>();

        for (int i = 0; i < sceneLinkedSMBs.Length; i++)
        {
            sceneLinkedSMBs[i].InternalInitialise(animator, monoBehaviour);
        }
    }

    protected void InternalInitialise(Animator animator, TMonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
        OnStart(animator);
    }

    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _firstFrameHappened = false;

        OnSLStateEnter(animator, stateInfo, layerIndex);
        OnSLStateEnter(animator, stateInfo, layerIndex, controller);
    }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (!animator.gameObject.activeSelf)
            return;

        if (animator.IsInTransition(layerIndex) && animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
        {
            OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex);
            OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex, controller);
        }

        if (!animator.IsInTransition(layerIndex) && _firstFrameHappened)
        {
            OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
            OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex, controller);
        }

        if (animator.IsInTransition(layerIndex) && !_lastFrameHappened && _firstFrameHappened)
        {
            _lastFrameHappened = true;

            OnSLStatePreExit(animator, stateInfo, layerIndex);
            OnSLStatePreExit(animator, stateInfo, layerIndex, controller);
        }

        if (!animator.IsInTransition(layerIndex) && !_firstFrameHappened)
        {
            _firstFrameHappened = true;

            OnSLStatePostEnter(animator, stateInfo, layerIndex);
            OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
        }

        if (animator.IsInTransition(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
        {
            OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);
            OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex, controller);
        }
    }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _lastFrameHappened = false;

        OnSLStateExit(animator, stateInfo, layerIndex);
        OnSLStateExit(animator, stateInfo, layerIndex, controller);
    }

    /// <summary>
    /// Called by a MonoBehaviour in the scene during its Start function.
    ///  씬의 모노비헤이비어가 호출하는 시작 함수.
    /// </summary>
    public virtual void OnStart(Animator animator) { }

    /// <summary>
    /// Called before Updates when execution of the state first starts (on transition to the state).
    /// </summary>
    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called after OnSLStateEnter every frame during transition to the state.
    /// 현재 스테이트로 들어오는 트랜지션이 처음 시작될 때 호출. OnSLStateEnter 이후 호출.
    /// </summary>
    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called on the first frame after the transition to the state has finished.
    /// 현제 스테이트로 들어오는 트렌지션이 완료된 이후 첫 번째 프레임에 호출.
    /// </summary>
    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called every frame after PostEnter when the state is not being transitioned to or from.
    /// 트랜지션이 끝난이 후 매 프레임마다 호출. 트랜지션이 없더라도 호출.
    /// </summary>
    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called on the first frame after the transition from the state has started.  Note that if the transition has a duration of less than a frame, this will not be called.
    /// 현재 스테이트에 나가는 트렌지션이 시작된 첫 번째 프레임에 호출. 트랜지션 지속시간이 없다면 함수는 호출되지 않는다.
    /// </summary>
    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called after OnSLStatePreExit every frame during transition to the state.
    /// 현재 스테이트를 나가는 트렌지션이 진행되는 동안 매 프레임마다 호출. OnSLStatePreExit 이후에 호출.
    /// </summary>
    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called after Updates when execution of the state first finshes (after transition from the state).
    /// 현재 스테이트에서 나가는 트렌지션이 완료될 때 호출.
    /// </summary>
    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called before Updates when execution of the state first starts (on transition to the state).
    /// </summary>
    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called after OnSLStateEnter every frame during transition to the state.
    /// </summary>
    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called on the first frame after the transition to the state has finished.
    /// </summary>
    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called every frame when the state is not being transitioned to or from.
    /// </summary>
    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called on the first frame after the transition from the state has started.  Note that if the transition has a duration of less than a frame, this will not be called.
    /// </summary>
    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called after OnSLStatePreExit every frame during transition to the state.
    /// </summary>
    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called after Updates when execution of the state first finshes (after transition from the state).
    /// </summary>
    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }
}

//This class repalce normal StateMachineBehaviour. It add the possibility of having direct reference to the object
//the state is running on, avoiding the cost of retrienving it through a GetComponent every time.
//c.f. Documentation for more in depth explainations.
public abstract class SealedSMB : StateMachineBehaviour
{
    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
