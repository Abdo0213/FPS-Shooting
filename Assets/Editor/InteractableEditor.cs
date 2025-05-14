using Unity.VisualScripting;
using UnityEditor;

[CustomEditor(typeof(Interactable),true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if(target.GetType() == typeof(EventOnlyInteractable)){
            interactable.promptMessage = EditorGUILayout.TextField("Prompt Message", interactable.promptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteractable can only use Unity Events", MessageType.Info);
            if(interactable.GetComponent<InteractEvent>() == null){
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractEvent>();
            }
        }
        else{
            base.OnInspectorGUI();
            if(interactable.useEvents){
                if(interactable.GetComponent<InteractEvent>() == null)
                    interactable.gameObject.AddComponent<InteractEvent>();
            }
            else{
                if(interactable.GetComponent<InteractEvent>() != null){
                    DestroyImmediate(interactable.GetComponent<InteractEvent>());
                }
            }
        }
    }
}
