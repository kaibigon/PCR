using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private float m_accerleration = 0.01f;
    private float m_maxVelY = 0.01f;
    private float m_RotationVel = 90f;
    private float m_velY = 0f;
    private Vector3 m_currentVel = Vector3.zero;
    private Transform shipTransform;

    // Start is called before the first frame update
    void Start()
    {
        shipTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (m_velY < m_maxVelY)
            {
                m_velY += m_accerleration * Time.deltaTime;
            }
        }
        else
        {
            m_velY -= m_accerleration * Time.deltaTime;
        }

        m_velY = Mathf.Clamp(m_velY, 0, m_maxVelY);
        m_currentVel = transform.up * m_velY;
        shipTransform.position += m_currentVel;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(m_RotationVel * Time.deltaTime * transform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-m_RotationVel * Time.deltaTime * transform.forward);
        }
    }

}