using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{

	public Sprite[] _sprites;
	public int FramesPerSprite = 6;
	public bool IsLooping = true;
	public bool ShouldDestroyOnEnd = false;

	private int index = 0;
	[SerializeField] Image image;
	private int frame = 0;

	public bool IsPlaying = false;

	public void LoadSprites(Sprite[] newSprites)
    {
		_sprites = newSprites;
    }


	void Update()
	{
		if (!IsPlaying) return;

		if (!IsLooping && index == _sprites.Length) return;
		frame++;
		if (frame < FramesPerSprite) return;
		image.sprite = _sprites[index];
		frame = 0;
		index++;
		if (index >= _sprites.Length)
		{
			if (IsLooping) index = 0;
			if (ShouldDestroyOnEnd) Destroy(gameObject);
		}
	}

	public void UnloadSprites()
    {
		_sprites = null;
		IsPlaying = false;
    }

}
