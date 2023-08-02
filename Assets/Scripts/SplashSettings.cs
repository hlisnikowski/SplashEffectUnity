using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item_setting")]
public class SplashSettings : ScriptableObject
{
	[Tooltip("XReducer will slow down horizontal axis ( left right top bottom movement )")]
	[Range(1f, 2.5f)]
	public float YReducer = 1.5f;

	[Tooltip("YReducer will slow down vertical axis ( height of the bounce )")]
	[Range(1f, 2.5f)]
	public float XReducer = 1.5f;

	public int numberOfBounces = 3;

	[Tooltip("Amount of vertical force")]
	public float velocity = 10;

	[Tooltip("Amount of horizontal force")]
	public float horizontalForce = 2;

	public float gravity = -30;

	[Tooltip("Tag of entity who can collect this item")]
	public string collectorTag = "Player";

	[Tooltip("When can Player pick up item")]
	public PickUpType pickUpType = PickUpType.AFTER;

	[Tooltip("It will create small shadow after last bounce")]
	public bool shadow = true;

	public float destroyTime = 0f;

    	[Tooltip("When Item hits the wall with that tag")]
   	public string[] colliderTags;

}
