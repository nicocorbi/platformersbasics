using UnityEngine;

public class presetchanger : MonoBehaviour
{
    [SerializeField] MovementStats[] statsList;
    int currentStatIndex = 0;
    [SerializeField] private Movement movimientoScript; // Referencia al script de Movimiento

    void Start()
    {
        // Asegúrate de tener la referencia al script de Movimiento
        if (movimientoScript == null)
        {
            movimientoScript = GetComponent<Movement>(); // Obtener referencia automáticamente si no se ha asignado
        }
        SetStatsProfile();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            currentStatIndex++;

            if (currentStatIndex >= statsList.Length)
            {
                currentStatIndex = 0;
            }
            SetStatsProfile();
        }
    }
    void SetStatsProfile()
    {
        movimientoScript.UpdateStat(statsList[currentStatIndex]);
        print("preset ha cambiado a:"+ statsList[currentStatIndex].name);
    }
}



