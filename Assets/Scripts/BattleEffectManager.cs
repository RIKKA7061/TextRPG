using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BattleEffectManager : MonoBehaviour
{
	public Image EnemyIcon; // Reference to the UI Image for the enemy icon
	public Image PlayerIcon; // Reference to the UI Image for the player icon
	private Color enemyOriginalColor; // To store the original color of the EnemyIcon
	private Color playerOriginalColor; // To store the original color of the PlayerIcon

	public GameObject damageTextPrefab; // Reference to the damage text prefab
	public Transform enemyTransform;    // Reference to enemy position
	public Transform playerTransform;   // Reference to player position
	public Transform parentTransform;

	public bool isPlayerTurn;
	private Coroutine blinkCoroutine;
	public float blinkSpeed = 1f; // 깜박임 속도 계수 (작을수록 느림)
	public float minAlpha = 0.5f; // 최소 알파값 (최대 어두움 값)

	public float DamageText_X_left = 2f;
	public float DamageText_X_Right = 2f;
	public float DamageText_Y_Down = 2f;
	public float DamageText_Y_Up = 2f;

	void Start()
	{
		// Store the original colors at the start
		enemyOriginalColor = EnemyIcon.color;
		playerOriginalColor = PlayerIcon.color;
	}

	public void EnemyHit()
	{
		Debug.Log("Enemy Blink");
		StartCoroutine(Blink(EnemyIcon, enemyOriginalColor));
	}

	public void PlayerHit()
	{
		Debug.Log("Player Blink");
		StartCoroutine(Blink(PlayerIcon, playerOriginalColor));
	}

	private IEnumerator Blink(Image icon, Color originalColor)
	{
		float duration = 0.25f; // Half of the 0.5s duration for each transition
		float elapsedTime = 0f;
		Color targetColor = Color.red;

		// Gradually change to red for the icon
		while (elapsedTime < duration)
		{
			icon.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		icon.color = targetColor; // Ensure it's exactly red

		// Gradually revert to the original color for the icon
		elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			icon.color = Color.Lerp(targetColor, originalColor, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		icon.color = originalColor; // Ensure it returns to the original color
	}

	public void ChangeEnemyToGray()
	{
		Debug.Log("회색처리");
		StartCoroutine(ChangeEnemyToGrayAfterDelay(0f, 0.5f)); // Start the coroutine with a 2-second delay
	}

	public void OriginalEnemyColor()
	{
		EnemyIcon.color = Color.white;
	}

	private IEnumerator ChangeEnemyToGrayAfterDelay(float delay, float transitionDuration)
	{
		// Wait for the specified delay
		yield return new WaitForSeconds(delay);

		Color originalColor = EnemyIcon.color; // Store the original color
		Color targetColor = Color.gray; // Target color (gray)

		float elapsedTime = 0f;

		// Gradually change to gray over the transitionDuration
		while (elapsedTime < transitionDuration)
		{
			EnemyIcon.color = Color.Lerp(originalColor, targetColor, elapsedTime / transitionDuration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure the color is exactly gray at the end
		EnemyIcon.color = targetColor;
	}

	// Call this method to display the damage text
	public void DamageText(int damage, string Case)
	{
		// Case -> damage text Pos
		Vector3 spawnPosition = Vector3.zero;

		switch (Case)
		{
			case "Enemy":
				spawnPosition = enemyTransform.position;
				break;
			case "Player":
				spawnPosition = playerTransform.position;
				break;
			default:
				Debug.LogWarning("Unrecognized case: " + Case);
				return;
		}

		Vector3 randomOffset = new Vector3(
		Random.Range(DamageText_X_left, DamageText_X_Right),  // Random offset for the X axis
		Random.Range(DamageText_Y_Down, DamageText_Y_Up),  // Random offset for the Y axis
		0f                      // Assuming it's 2D (Z axis is 0, or you can adjust if in 3D)
		);

		Vector3 randomPosition = spawnPosition + randomOffset;

		GameObject damageTextObj = Instantiate(damageTextPrefab, randomPosition, Quaternion.identity);
		damageTextObj.transform.SetParent(parentTransform, false);

		TextMeshProUGUI damageText = damageTextObj.GetComponent<TextMeshProUGUI>();

		// Damage Text Num Input
		damageText.text = damage.ToString();

		// Text Animation Start
		StartCoroutine(FadeOutDamageText(damageTextObj));
	}

	// Coroutine to handle fading and moving the damage text
	private IEnumerator FadeOutDamageText(GameObject damageTextObj)
	{
		TextMeshProUGUI damageText = damageTextObj.GetComponent<TextMeshProUGUI>();

		// Move Up
		float moveDuration = 1f; // Text moves up for 1 second
		float fadeDuration = 1f; // Text fades out for 1 second after stopping
		float pauseDuration = 1f; // Pause before fading out

		Vector3 startPosition = damageTextObj.transform.position;
		Vector3 targetPosition = startPosition + Vector3.up * 50f; // Move up 50 units

		Color startColor = damageText.color;
		Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Invisible

		float elapsedTime = 0f;

		// Step 1: Move Up
		while (elapsedTime < moveDuration)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / moveDuration;

			// Lerp position
			damageTextObj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

			yield return null;
		}

		// Step 2: Pause
		yield return new WaitForSeconds(pauseDuration);

		// Step 3: Fade Out
		elapsedTime = 0f;
		while (elapsedTime < fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / fadeDuration;

			// Lerp color
			damageText.color = Color.Lerp(startColor, targetColor, t);

			yield return null;
		}

		// Step 4: Destroy after animation
		Destroy(damageTextObj);
	}

	public void PlayerTurnBlinkStart()
	{
		isPlayerTurn = true;

		// 이미 깜박임 코루틴이 실행 중이라면 중지
		if (blinkCoroutine != null)
		{
			StopCoroutine(blinkCoroutine);
		}

		// 깜박임 코루틴 시작
		blinkCoroutine = StartCoroutine(BlinkEffect());
	}

	public void PlayerTurnBlinkStop()
	{
		isPlayerTurn = false;

		// 깜박임 코루틴 중지
		if (blinkCoroutine != null)
		{
			StopCoroutine(blinkCoroutine);
			blinkCoroutine = null;
		}

		// 아이콘을 기본 상태로 복구
		if (PlayerIcon != null)
		{
			Color color = PlayerIcon.color;
			color.a = 1f; // 알파 값 최대
			PlayerIcon.color = color;
		}
	}


	private IEnumerator BlinkEffect()
	{
		float alpha = 1f; // 초기 알파값
		bool decreasing = true;

		while (isPlayerTurn)
		{
			// 알파 값을 증가 또는 감소
			if (decreasing)
			{
				alpha -= Time.deltaTime * blinkSpeed; // 감소 속도
				if (alpha <= minAlpha) // 최소 알파값 도달
				{
					decreasing = false;
				}
			}
			else
			{
				alpha += Time.deltaTime * blinkSpeed; // 증가 속도
				if (alpha >= 1f) // 최대 알파값 도달
				{
					decreasing = true;
				}
			}

			// 알파 값을 PlayerIcon에 적용
			if (PlayerIcon != null)
			{
				Color color = PlayerIcon.color;
				color.a = alpha;
				PlayerIcon.color = color;
			}

			yield return null; // 다음 프레임까지 대기
		}
	}



}
