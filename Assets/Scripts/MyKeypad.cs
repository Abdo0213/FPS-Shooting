using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MyKeypad : Interactable
{
    [SerializeField] private GameObject door;
    [SerializeField] private int targetSceneIndex = 2; // Default fallback
    [SerializeField] private Target[] enemies;
    public LoadingMenuManager loadingMenuManager;
    private bool doorOpen;
    private bool canInteract = true;

    protected override void Interact()
    {
        if (!canInteract || door == null) return;

        StartCoroutine(TryOpenDoorRoutine());
    }

    private IEnumerator TryOpenDoorRoutine()
    {
        canInteract = false;
        if (!AllEnemiesDead())
        {
            Debug.LogWarning("Enemies remain! Cannot open door.");
            canInteract = true;
            yield break;
        }
        doorOpen = !doorOpen;
        Animator animator = door.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isOpen", doorOpen);
        }
        else
        {
            Debug.LogError("No Animator found on door!");
        }
        yield return new WaitForSeconds(2f);
        DataPersistenceManager.instance.StartFreshAtLevel(targetSceneIndex);
    }

    private bool AllEnemiesDead()
    {
        if (enemies == null || enemies.Length == 0) return true;

        foreach (Target enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy && !enemy.IsDead)
            {
                return false;
            }
        }
        return true;
    }
}