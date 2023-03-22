using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Loot : MonoBehaviour
{
	// You can chose to use Scriptable Object or Serialized.
	// One settings for SplashSetting is already created
	// [SerializeField] SplashSettings settings;	// Scriptable Object
	[SerializeField] LootSettings settings;			// Serialized
	
	private int bounces = 0;
	private bool isGrounded = true;
	private Vector2 groundVelocity;
	private float verticalVelocity, afterVelocity;

	private Transform t_parent; // Main
	private Transform t_body; // Body
	private Transform t_shadow; // Shadow

	#region OPTIONAL PICK UP
	bool canCollect;
	// Make it trigger
	BoxCollider2D pickUpCollision;

	// Dont forget to add rigidbody to the player and right Tag
	private void PickUp(Collider2D collision)
	{
		// Here write your logic like ... collision.GetComponent<PlayerInvetory>();
		print($"{this.gameObject.name} has been picked up");
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(settings.collectorTag) && canCollect)
			PickUp(collision);
	}
	#endregion

	void Start()
	{
		if(settings.destroyTime > 0)
			Destroy(this.gameObject, settings.destroyTime);

		pickUpCollision = GetComponent<BoxCollider2D>();
		pickUpCollision.enabled = false;

		// Sprite has to be disabled, its sprite is only for t_body sprite
		GetComponent<SpriteRenderer>().enabled = false;
		CreateBody();
		CreateShadow();
		SimulateDrop();
	}

	void Update()
	{
		UpdatePosition();
	}

	void Initialize(Vector2 groundvelocity)
	{
		isGrounded = false;
		// Slow down the height of bounce
		afterVelocity /= settings.YReducer;
		this.groundVelocity = groundvelocity;
		this.verticalVelocity = afterVelocity;
		bounces++;
	}


	// Call this method to simulate bounce effect
	// On Default it's in the Start()
	public void SimulateDrop()
	{
		StartCoroutine(Simulate());
	}

	private IEnumerator Simulate()
	{
	    yield return new WaitForSeconds(1f);
		groundVelocity = new Vector2(Random.Range(-settings.horizontalForce, settings.horizontalForce), Random.Range(-settings.horizontalForce, settings.horizontalForce));
		verticalVelocity = Random.Range(settings.velocity - 1, settings.velocity);
		afterVelocity = verticalVelocity;
		Initialize(groundVelocity);

		if (settings.pickUpType == PickUpType.IMMEDIATELY)
			ChangeItemToBeCollectable();

		yield return null;
	}

	private void UpdatePosition()
	{
		if (!isGrounded)
		{
			verticalVelocity += settings.gravity * Time.deltaTime;
		    t_body.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
			t_parent.position += (Vector3)groundVelocity * Time.deltaTime;
			CheckGroundHit();
		}
	}

	/// <summary>
	/// If number of bounces is less than current bounces, it will add force to the item
	/// Force is each bounce reduced by XReducer and YReducer
	/// </summary>
	private void CheckGroundHit()
	{
		if (t_body.position.y < t_shadow.position.y)
		{
			t_body.position = t_shadow.position;

			if (bounces < settings.numberOfBounces)
			{
				Initialize(new Vector2(groundVelocity.x / settings.XReducer, groundVelocity.y / settings.XReducer));
			}
			else {

				// Item can be collected
				if (settings.pickUpType == PickUpType.AFTER)
					ChangeItemToBeCollectable();

				// Give item shadow after last bounce
				if (settings.shadow)
					t_shadow.position = new Vector3(t_shadow.position.x, t_shadow.position.y - 0.05f, t_shadow.position.z);

				// Prevent item moving
				isGrounded = true;
			}
			
		}
	}

	private void ChangeItemToBeCollectable()
	{
		pickUpCollision.enabled = true;
		canCollect = true;
	}

	#region SPRITE RENDER PARTS 

	private SpriteRenderer sprRndCaster;
	private SpriteRenderer sprRndBody;
	private SpriteRenderer sprRndShadow;

	/// <summary>
	/// Will create a icon Sprite Renderer to a Parent
	/// </summary>
	void CreateBody()
	{
		t_parent = transform;
		t_body = new GameObject().transform;
		t_body.parent = t_parent;
		t_body.gameObject.name = "Body";
		t_body.localRotation = Quaternion.identity;
		t_body.localPosition = Vector3.zero;
		sprRndCaster = GetComponent<SpriteRenderer>();
		sprRndBody = t_body.gameObject.AddComponent<SpriteRenderer>();
		sprRndBody.sortingLayerName = sprRndCaster.sortingLayerName;
		sprRndBody.sortingOrder = sprRndCaster.sortingOrder;
		sprRndBody.sprite = sprRndCaster.sprite;
	}

	/// <summary>
	/// Will create a shadow Sprite Renderer to a Parent
	/// </summary>
	void CreateShadow()
	{
		t_parent = transform;
		t_shadow = new GameObject().transform;
		t_shadow.parent = t_parent;
		t_shadow.gameObject.name = "Shadow";
		t_shadow.localRotation = Quaternion.identity;
		t_shadow.localPosition = Vector3.zero;
		sprRndCaster = GetComponent<SpriteRenderer>();
		sprRndShadow = t_shadow.gameObject.AddComponent<SpriteRenderer>();
		sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
		sprRndShadow.sortingOrder = sprRndCaster.sortingOrder - 1;
		sprRndShadow.color = Color.black;
		sprRndShadow.sprite = sprRndCaster.sprite;
	}

	#endregion
}

// You can also make it as Scriptable Object.
[System.Serializable]
public class LootSettings
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

	
}

public enum PickUpType
{
	IMMEDIATELY,
	NEVER,
	AFTER
}


