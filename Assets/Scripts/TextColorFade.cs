using System.Collections;
using TMPro;
using UnityEngine;

public class TextColorFade : MonoBehaviour
{
    // Touch To Start 연출 스크립트

    private TextMeshProUGUI textMeshPro;
    public float fadeDuration = 1f; // 한 번에 변화하는 시간
    public float holdTime = 1f; // 투명해진 상태를 유지하는 시간

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeText());
    }

    public void Restart()
    {
		textMeshPro = GetComponent<TextMeshProUGUI>();
		StartCoroutine(FadeText());
	}

    private IEnumerator FadeText()
    {
        // 텍스트 색상 처음 (하얀색)
        Color startColor = textMeshPro.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // 투명하게 만들기

        // 투명으로 바뀌는 애니메이션
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            textMeshPro.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMeshPro.color = targetColor;

        // 잠시 대기 (투명 상태 유지)
        yield return new WaitForSeconds(holdTime);

        // 다시 하얀색으로 돌아오는 애니메이션
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            textMeshPro.color = Color.Lerp(targetColor, startColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMeshPro.color = startColor;

        // 반복하고 싶다면 다시 호출
        StartCoroutine(FadeText());
    }
}
