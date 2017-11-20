using UnityEngine;
//Controlles de notre joueur   = Importé de Unity
public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			
	[HideInInspector]
	public bool jump = false;               
    [HideInInspector]
    public bool sit = false;                

    public float moveForce = 365f;			
	public float maxSpeed = 5f;				
	public AudioClip[] jumpClips;			
	public float jumpForce = 1000f;			

	private Transform groundCheck;			
	private bool grounded = false;			
	private Animator anim;					


	void Start() // Instancier le joueur à faire bouger
	{
        groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}


    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")); // Savoir si le joueur est sur le sol
        //Si le joueur est au sol et que on  appuie sur espace, on saute
        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;
        if (Input.GetButtonDown("Down"))
            sit = true;
        if (Input.GetButtonUp("Down"))
            sit = false;
    }


    void FixedUpdate () // Bouger le bonhomme et mettre des animations avec ses mouvements
	{
        float h=0;
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Base Layer.Sit"))
             h = Input.GetAxis("Horizontal");
		anim.SetFloat("Speed", Mathf.Abs(h));
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        if (h > 0 && !facingRight)
			Flip();
		else if(h < 0 && facingRight)
			Flip();
        if (jump)
		{
			anim.SetTrigger("Jump");
			int i = Random.Range(0, jumpClips.Length);
			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
        anim.SetBool("Sit",sit);
    }	
	void Flip () // Changer sa direction selon ou on va
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
