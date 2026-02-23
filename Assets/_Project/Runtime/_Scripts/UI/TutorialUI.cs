using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    VisualElement icon;
    Coroutine routine;

    void Awake()
    {
        Instance = this;

        var doc = GetComponent<UIDocument>();
        icon = doc.rootVisualElement.Q<VisualElement>("TutorialIndicator");
    }

    public void ShowIndicator(List<Sprite> sprites, float animSpeed, float animationTime)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(AnimateSpriteUI(sprites, animSpeed, animationTime));
    }
    IEnumerator AnimateSpriteUI(List<Sprite> sprites, float animSpeed, float animationTime)
    {
        icon.style.opacity = 1;
        float timer = animationTime;

        while (timer > 0)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                icon.style.backgroundImage = new StyleBackground(sprites[i]);
                yield return new WaitForSeconds(animSpeed);

                timer -= animSpeed;
            }
        }
        icon.style.opacity = 0;
        routine = null;
    }

}
