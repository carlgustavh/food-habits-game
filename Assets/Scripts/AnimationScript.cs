using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator playerAnimator;

    private bool isJumping = false; // Flag to track if the player is currently jumping
    private bool isRolling = false; // Flag to track if the player is currently rolling

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the game object
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAnimator != null)
        {
            // Check for Space key press and if not already jumping
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                // Trigger the "TrJump" animation
                playerAnimator.SetTrigger("TrJump");
            }

            // Check for S key press and if not already rolling
            if (Input.GetKeyDown(KeyCode.LeftControl) && !isRolling)
            {
                // Trigger the "TrRoll" animation
                playerAnimator.SetTrigger("TrRoll");
            }

            // Update the isJumping flag based on the current animation state
            isJumping = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump");
            // Update the isRolling flag based on the current animation state
            isRolling = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Roll");
        }
    }
}
