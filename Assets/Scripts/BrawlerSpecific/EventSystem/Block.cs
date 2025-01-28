/*
 * Copyright (C) 2020-2023 Tilt Five, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : ActionEvent
{
    bool isCancelled = false;
    public override IEnumerator Perform()
    {
        m_Animator.ResetTrigger("EndBlock");
        //Prevent movement while the jump begins
        controller.movementScale = 0.01f;
        if (!controller.ChangeActionState(newActionState))
        {
            CancelEvent();
            yield break;
        }


        m_Animator.SetTrigger(animationTrigger);
        var character = controller.controlledCharacter as Character;
        
        while (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName(animationState))
        {
            if (isCancelled)
            {
                yield break;
            }
            yield return null;
        }
        controller.isBlocking = true;
        character.shieldObject.SetActive(true);
        for (int i = 0; i < duration; i++)
        {
            character.shieldStrength -= 1.0f / 6.0f;

            if(character.shieldStrength <= 0)
            {
                character.shieldObject.SetActive(false);
                CancelEvent();
                yield break;
            }
            if (isCancelled)
            {
                character.shieldObject.SetActive(false);
                //CancelEvent();
                yield break;
            }
            duration += 1;
            yield return null;
        }
        character.shieldObject.SetActive(false);
        //Resolve the event
        CancelEvent();
    }

    public override void CancelEvent()
    {
        m_Animator.SetTrigger("EndBlock");
        controller.isBlocking = false;
        isCancelled = true;
        controller.ChangeActionState(PlayerController.ActionState.Neutral);
        controller.currentEvent = null;
        canBeInterrupted = true;
        controller.movementScale = 1.0f;
        controller.rotationScale = 1.0f;
        controller.enableQueue = true;
    }

    public Block(Rigidbody rb, Animator m_Animator, PlayerController controller)
    {
        this.rb = rb;
        this.canBeInterrupted = true;
        this.controller = controller;
        this.m_Animator = m_Animator;
        this.newActionState = PlayerController.ActionState.Blocking;
        this.animationState = "Blocking";
        this.animationTrigger = "Blocking";
        this.duration = 10;
    }
}