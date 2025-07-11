using UnityEngine;

public class Tracker : MonoBehaviour
{
    void Update()
    {
        // Verificar si se hace clic con el bot�n izquierdo del rat�n
        if (Input.GetMouseButtonDown(0))
        {
            // Lanzar un rayo desde la posici�n del rat�n
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Comprobar si el rayo interseca con alg�n objeto
            if (Physics.Raycast(ray, out hit))
            {
                // Obtener el nombre del objeto golpeado
                string objectName = hit.collider.gameObject.name;

                // Imprimir el nombre del objeto en la consola
                Debug.Log("Objeto apuntado: " + objectName);
            }
        }
    }
}
