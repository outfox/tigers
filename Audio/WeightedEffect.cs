//SPDX-License-Identifier: Unlicense

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tiger.Audio
{
	[CreateAssetMenu(menuName="Audio/Weighted Random Effect")]
	[Icon("Assets/Tiger/Audio/Editor/Icons/sound.png")]
	public class WeightedRandomEffect : AudioEvent
	{
		[Serializable]
		public struct WeightedEntry
		{
			public AudioEvent item;
			public float weight;
		}

		public WeightedEntry[] randomized;

		public override float Play(AudioSource source)
		{
			float totalWeight = 0;
			for (var i = 0; i < randomized.Length; ++i)
			{
				totalWeight += randomized[i].weight;
			}

			var pick = Random.Range(0f, totalWeight);
			for (var i = 0; i < randomized.Length; ++i)
			{
				if (pick <= randomized[i].weight) return randomized[i].item.Play(source);
				pick -= randomized[i].weight;
			}
			return 0;
		}
	}
}