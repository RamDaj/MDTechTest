using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;


    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private IInteractable _interactable;


    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            int minIndex = 0, i = 0;
            float MinDistance = Mathf.Infinity;
            foreach (Collider _interact in _colliders)
            {
                if (_interact != null)
                {
                    float dist = Vector3.Distance(_interact.gameObject.transform.position, _interactionPoint.position);
                    if (dist < MinDistance)
                    {
                        MinDistance = dist; minIndex = i;
                    }
                    i++;
                }
            }
            var _interactable = _colliders[minIndex].GetComponent<IInteractable>();


            if (_interactable != null)
            {
                if (!_interactionPromptUI.isDisplayed && _interactable.CanInteract()) _interactionPromptUI.SetUpInteract();
                else if (_interactionPromptUI.isDisplayed && !_interactable.CanInteract()) _interactionPromptUI.CloseInteract();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    _interactable.Interact(this);
                }

            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
            if (_interactionPromptUI.isDisplayed) _interactionPromptUI.CloseInteract();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }

    public LayerMask GetLayerMask()
    {
        return _interactableMask;
    }
}
