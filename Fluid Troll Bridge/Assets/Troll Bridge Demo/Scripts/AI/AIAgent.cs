
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FluidHTN;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AIAgent : MonoBehaviour
{
    [SerializeField][Tooltip("The domain definition for this agent")]
    private AIDomainDefinition _domainDefinition;

    [SerializeField] [Tooltip("The sensing capabilities of our agent")]
    private AISenses _senses;

    [SerializeField] [Tooltip("A Transform representing the head of the agent")]
    private Transform _head;

    private Planner<AIContext> _planner;
    private Domain<AIContext> _domain;
    private AIContext _context;
    private SensorySystem _sensory;

    private void Awake()
    {
        if (_domainDefinition == null)
        {
            Debug.LogError($"Missing domain definition in {name}!");
            gameObject.SetActive(false);
            return;
        }

        _planner = new Planner<AIContext>();
        _context = new AIContext(this, _senses, _head, GetComponent<Animator>(), GetComponent<NavMeshAgent>());
        _sensory = new SensorySystem(this);

        _domain = _domainDefinition.Create();

        _context.PlannerState.OnNewPlan += tasks => Debug.Log($"{name} - New plan: {ToString(tasks)}");
        _context.PlannerState.OnNewTask += task => Debug.Log($"{name} - New task: {task.Name}");
        _context.PlannerState.OnCurrentTaskStarted += task => Debug.Log($"{name} - Task started: {task.Name}");
        _context.PlannerState.OnCurrentTaskCompletedSuccessfully += task => Debug.Log($"{name} - Task finished: {task.Name}");
        _context.PlannerState.OnCurrentTaskFailed += task => Debug.Log($"{name} - Task failed: {task.Name}");
    }

    private string ToString(Queue<ITask> plan)
    {
        var planArray = plan.ToArray();
        var sb = new StringBuilder();
        for (var i = 0; i < planArray.Length; i++)
        {
            var task = planArray[i];
            sb.Append( task.Name );
            if (i < planArray.Length - 1)
            {
                sb.Append(" -> ");
            }
        }
        return sb.ToString();
    }

    private void Update()
    {
        if (_planner == null || _domain == null || _context == null || _sensory == null)
            return;

        _context.Time = Time.time;
        _context.DeltaTime = Time.deltaTime;

        _context.Animator.SetFloat("speed", _context.NavAgent.desiredVelocity.magnitude);

        if (_context.CanSense)
        {
            _sensory.Tick(_context);
        }

        _planner.Tick(_domain, _context);
    }

    private void OnDrawGizmos()
    {
        if (_context == null)
            return;

        _senses?.DrawGizmos(_context);
        _sensory?.DrawGizmos(_context);

#if UNITY_EDITOR
        if (_context.PlannerState.CurrentTask != null)
        {
            Handles.Label(_context.Head.transform.position + Vector3.up, _context.PlannerState.CurrentTask.Name);
        }
#endif
    }
}