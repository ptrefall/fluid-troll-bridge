
using System;
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
    }

    private void Update()
    {
        if (_planner == null || _domain == null || _context == null || _sensory == null)
            return;

        _context.Time = Time.time;
        _context.DeltaTime = Time.deltaTime;

        _context.Animator.SetFloat("speed", _context.NavAgent.desiredVelocity.magnitude);

        _sensory.Tick(_context);
        _planner.Tick(_domain, _context);
    }

    private void OnDrawGizmos()
    {
        if (_context == null)
            return;

        _senses?.DrawGizmos(_context);
        _sensory?.DrawGizmos(_context);

#if UNITY_EDITOR
        var task = _planner.GetCurrentTask();
        if (task != null)
        {
            Handles.Label(_context.Head.transform.position + Vector3.up, task.Name);
        }
#endif
    }
}