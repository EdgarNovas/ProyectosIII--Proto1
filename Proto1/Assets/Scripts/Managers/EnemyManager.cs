using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    // Ahora guardamos referencias a los StateMachines
    private List<EnemyStateMachine> enemies = new List<EnemyStateMachine>();


    // Un contador para saber cuántos enemigos son "parreables" AHORA MISMO.
    private int parryableEnemiesCount = 0;

    // Eventos para notificar al jugador.
    public event Action OnFirstParryWindowOpened;
    public event Action OnLastParryWindowClosed;

    /// <summary>
    /// Los enemigos llaman a este método para informar de su estado de "parry".
    /// </summary>
    public void ReportParryableStatus(bool isNowParryable)
    {
        if (isNowParryable)
        {
            // Un enemigo más ha entrado en la ventana de parry.
            parryableEnemiesCount++;

            // Si este es el PRIMER enemigo, disparamos el evento para MOSTRAR el indicador.
            if (parryableEnemiesCount >= 1)
            {
                OnFirstParryWindowOpened?.Invoke();
            }
        }
        else
        {
            // Un enemigo ha salido de la ventana de parry.
            parryableEnemiesCount--;

            // Si el contador llega a CERO, significa que ya NO QUEDAN enemigos "parreables".
            // Disparamos el evento para OCULTAR el indicador.
            if (parryableEnemiesCount == 0)
            {
                OnLastParryWindowClosed?.Invoke();
            }
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            // Si ya existe un manager, este se destruye
            Destroy(gameObject);
        }
        else
        {
            // Si no existe, este se convierte en la instancia
            Instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(AI_Loop());
    }

    void OnDestroy()
    {
        StopCoroutine(AI_Loop());
    }

    IEnumerator AI_Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));

            // Elegir un enemigo disponible para atacar
            EnemyStateMachine attackingEnemy = GetAvailableEnemy();

            if (attackingEnemy != null)
            {
                // Le ordenamos que cambie al estado de ataque
                attackingEnemy.SwitchState(typeof(EnemyChaseState));
            }
        }
    }

    private EnemyStateMachine GetAvailableEnemy()
    {
        List<EnemyStateMachine> availableEnemies = new List<EnemyStateMachine>();

        Transform player = GameManager.Instance.GetPlayer();
        if (player == null) { return null; } // Can't find enemies if there's no player.

        foreach (var enemy in enemies)
        {

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.position);
            // Un enemigo está disponible si está vivo y en el estado Idle
            if (enemy.Health > 0 && enemy.GetCurrentState().GetType() == typeof(EnemyIdleState) && distanceToPlayer <= enemy.DetectionRange)
            {
                availableEnemies.Add(enemy);
            }
        }

        if (availableEnemies.Count == 0) return null;

        // Elige uno al azar de los disponibles
        return availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];
    }
    
    public void PrepareEnemyForHit(EnemyStateMachine target)
    {
        target.SwitchState(typeof(EnemyWaitForHitState));
    }

    public void GetHit(EnemyStateMachine target)
    {
        target.SwitchState(typeof(EnemyHitState));
    }

    public void AddEnemy(EnemyStateMachine enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyStateMachine enemy)
    { 
        enemies.Remove(enemy);
    }

    public bool IsOnlyEnemy()
    {
        return enemies.Count < 2;
    }

    public List<EnemyStateMachine> GetEnemys()
    {
        return enemies;
    }

    /// <summary>
    /// Busca en la lista de enemigos activos si alguno es un objetivo válido para un parry.
    /// </summary>
    /// <param name="playerTransform">La posición y rotación del jugador.</param>
    /// <param name="parryAngle">El ángulo (en grados) del cono frontal del jugador para el parry.</param>
    /// <returns>El primer enemigo que cumpla las condiciones, o null si no hay ninguno.</returns>
    public EnemyStateMachine GetParryableEnemy(Transform playerTransform, float parryAngle,float maxDistance)
    {
        // Recorremos todos los enemigos activos en la escena.
        foreach (EnemyStateMachine enemy in enemies)
        {
            // 1. Primera condición: ¿Está el enemigo en su ventana de ataque "parreable"?
            // Esta bandera la controla el 'EnemyAttackState' del enemigo.
            if (!enemy.IsInParryableWindow)
            {
                continue; // Si no es parreable, pasamos al siguiente enemigo de la lista.
            }

            float distanceToPlayer = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distanceToPlayer > maxDistance)
            {
                continue; // Si está demasiado lejos, lo ignoramos y pasamos al siguiente.
            }

            // 2. Segunda condición: ¿Está el jugador mirando hacia el enemigo?
            // Calculamos el vector que va desde el jugador hacia el enemigo.
            Vector3 directionToEnemy = (enemy.transform.position - playerTransform.position).normalized;

            // Calculamos el ángulo entre la dirección a la que mira el jugador y la dirección hacia el enemigo.
            float angle = Vector3.Angle(playerTransform.forward, directionToEnemy);

            // Si el ángulo es menor que el permitido, significa que el jugador está encarado al enemigo.
            if (angle <= parryAngle)
            {
                // ¡Hemos encontrado un objetivo válido! Lo devolvemos inmediatamente.
                return enemy;
            }
        }

        // Si el bucle termina y no hemos encontrado ningún enemigo que cumpla las condiciones, devolvemos null.
        return null;
    }

    /*
    private EnemyScript[] enemies;
    public EnemyStruct[] allEnemies;
    private List<int> enemyIndexes;

    [Header("Main AI Loop - Settings")]
    private Coroutine AI_Loop_Coroutine;

    public int aliveEnemyCount;
    void Start()
    {
        enemies = GetComponentsInChildren<EnemyScript>();

        allEnemies = new EnemyStruct[enemies.Length];

        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].enemyScript = enemies[i];
            allEnemies[i].enemyAvailability = true;
        }

        StartAI();
    }

    public void StartAI()
    {
        AI_Loop_Coroutine = StartCoroutine(AI_Loop(null));
    }

    IEnumerator AI_Loop(EnemyScript enemy)
    {
        if (AliveEnemyCount() == 0)
        {
            StopCoroutine(AI_Loop(null));
            yield break;
        }

        yield return new WaitForSeconds(Random.Range(.5f,1.5f));

        EnemyScript attackingEnemy = RandomEnemyExcludingOne(enemy);

        if (attackingEnemy == null)
            attackingEnemy = RandomEnemy();

        if (attackingEnemy == null)
            yield break;
            
        yield return new WaitUntil(()=>attackingEnemy.IsRetreating() == false);
        yield return new WaitUntil(() => attackingEnemy.IsLockedTarget() == false);
        yield return new WaitUntil(() => attackingEnemy.IsStunned() == false);

        attackingEnemy.SetAttack();

        yield return new WaitUntil(() => attackingEnemy.IsPreparingAttack() == false);

        attackingEnemy.SetRetreat();

        yield return new WaitForSeconds(Random.Range(0,.5f));

        if (AliveEnemyCount() > 0)
            AI_Loop_Coroutine = StartCoroutine(AI_Loop(attackingEnemy));
    }

    public EnemyScript RandomEnemy()
    {
        enemyIndexes = new List<int>();

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyAvailability)
                enemyIndexes.Add(i);
        }

        if (enemyIndexes.Count == 0)
            return null;

        EnemyScript randomEnemy;
        int randomIndex = Random.Range(0, enemyIndexes.Count);
        randomEnemy = allEnemies[enemyIndexes[randomIndex]].enemyScript;

        return randomEnemy;
    }

    public EnemyScript RandomEnemyExcludingOne(EnemyScript exclude)
    {
        enemyIndexes = new List<int>();

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyAvailability && allEnemies[i].enemyScript != exclude)
                enemyIndexes.Add(i);
        }

        if (enemyIndexes.Count == 0)
            return null;

        EnemyScript randomEnemy;
        int randomIndex = Random.Range(0, enemyIndexes.Count);
        randomEnemy = allEnemies[enemyIndexes[randomIndex]].enemyScript;

        return randomEnemy;
    }

    public int AvailableEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyAvailability)
                count++;
        }
        return count;
    }

    /*
    public bool AnEnemyIsPreparingAttack()
    {
        foreach (EnemyStruct enemyStruct in allEnemies)
        {
            if (enemyStruct.enemyScript.IsPreparingAttack())
            {
                return true;
            }
        }
        return false;
    }


    public int AliveEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyScript.isActiveAndEnabled)
                count++;
        }
        aliveEnemyCount = count;
        return count;
    }

    public void SetEnemyAvailiability (EnemyScript enemy, bool state)
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyScript == enemy)
                allEnemies[i].enemyAvailability = state;
        }

        if (FindObjectOfType<EnemyDetection>().CurrentTarget() == enemy)
            FindObjectOfType<EnemyDetection>().SetCurrentTarget(null);
    }

    */
}

[System.Serializable]
public struct EnemyStruct
{
    public EnemyScript enemyScript;
    public bool enemyAvailability;
}
