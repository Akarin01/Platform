using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healTextPrefab;
    Canvas canvas;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    private void OnEnable()
    {
        CharacterEvents.onCharacterDameged.AddListener(OnCharacterDamaged);
        CharacterEvents.onCharacterHealed.AddListener(OnCharacterHealed);
    }

    private void OnDisable()
    {
        CharacterEvents.onCharacterDameged.RemoveListener(OnCharacterDamaged);
        CharacterEvents.onCharacterHealed.RemoveListener(OnCharacterHealed);
    }

    public void OnCharacterDamaged(GameObject character, int damage)
    {
        Vector3 characterPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TextMeshProUGUI damageText = Instantiate(damageTextPrefab, characterPosition, Quaternion.identity, canvas.transform).GetComponent<TextMeshProUGUI>();

        damageText.text = damage.ToString();
    }

    public void OnCharacterHealed(GameObject character, int heal)
    {
        Vector3 characterPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TextMeshProUGUI healText = Instantiate(healTextPrefab, characterPosition, Quaternion.identity, canvas.transform).GetComponent<TextMeshProUGUI>();

        healText.text = heal.ToString();
    }

    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + ":" + this.GetType() + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif

#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
            Application.Quit();
#endif
        }
    }
}
