using System.Collections;
using TMPro;
using UnityEngine;

public class TextColorFade : MonoBehaviour
{
    // Touch To Start ���� ��ũ��Ʈ

    private TextMeshProUGUI textMeshPro;
    public float fadeDuration = 1f; // �� ���� ��ȭ�ϴ� �ð�
    public float holdTime = 1f; // �������� ���¸� �����ϴ� �ð�

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
        // �ؽ�Ʈ ���� ó�� (�Ͼ��)
        Color startColor = textMeshPro.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // �����ϰ� �����

        // �������� �ٲ�� �ִϸ��̼�
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            textMeshPro.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMeshPro.color = targetColor;

        // ��� ��� (���� ���� ����)
        yield return new WaitForSeconds(holdTime);

        // �ٽ� �Ͼ������ ���ƿ��� �ִϸ��̼�
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            textMeshPro.color = Color.Lerp(targetColor, startColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMeshPro.color = startColor;

        // �ݺ��ϰ� �ʹٸ� �ٽ� ȣ��
        StartCoroutine(FadeText());
    }
}
