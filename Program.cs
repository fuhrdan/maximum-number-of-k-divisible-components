//*****************************************************************************
//** 2872. Maximum Number of K-Divisible Components                 leetcode **
//*****************************************************************************

typedef struct Node {
    int vertex;
    struct Node* next;
} Node;

typedef struct {
    Node** list;
    int size;
} AdjacencyList;

Node* createNode(int vertex) {
    Node* newNode = (Node*)malloc(sizeof(Node));
    newNode->vertex = vertex;
    newNode->next = NULL;
    return newNode;
}

AdjacencyList* createAdjacencyList(int size) {
    AdjacencyList* adj = (AdjacencyList*)malloc(sizeof(AdjacencyList));
    adj->list = (Node**)calloc(size, sizeof(Node*));
    adj->size = size;
    return adj;
}

void addEdge(AdjacencyList* adj, int src, int dest) {
    Node* newNode = createNode(dest);
    newNode->next = adj->list[src];
    adj->list[src] = newNode;

    newNode = createNode(src);
    newNode->next = adj->list[dest];
    adj->list[dest] = newNode;
}

long dfs(AdjacencyList* adj, int* values, int k, int* count, int curr, int parent) {
    long sum = values[curr];

    Node* temp = adj->list[curr];
    while (temp != NULL) {
        if (temp->vertex != parent) {
            sum += dfs(adj, values, k, count, temp->vertex, curr);
        }
        temp = temp->next;
    }

    sum %= k;
    if (sum == 0) {
        (*count)++;
    }
    return sum;
}

int maxKDivisibleComponents(int n, int** edges, int edgesSize, int* edgesColSize, int* values, int valuesSize, int k) {
    AdjacencyList* adj = createAdjacencyList(n);
    for (int i = 0; i < edgesSize; i++) {
        addEdge(adj, edges[i][0], edges[i][1]);
    }

    int count = 0;
    dfs(adj, values, k, &count, 0, -1);

    for (int i = 0; i < adj->size; i++) {
        Node* temp = adj->list[i];
        while (temp != NULL) {
            Node* toFree = temp;
            temp = temp->next;
            free(toFree);
        }
    }
    free(adj->list);
    free(adj);

    return count;
}