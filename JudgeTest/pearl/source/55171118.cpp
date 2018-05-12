#include <iostream>
#include <fstream>
#include <algorithm>
#include <queue>

using namespace std;

int result;

struct Node{
    int data;
    int count;
    Node * left;
    Node * right;

    Node()  {    count = 0;  left = right = NULL;  }
};

struct cmp{
    bool operator()(const Node * x, const Node * y)
    {
        return x->count > y->count;
    }
};

Node * CreateHuffman(Node * a, Node * b)
{
    Node * ret = new Node;
    ret->data = '#';
    ret->count = a->count + b->count;
    ret->left = a;
    ret->right = b;
    return ret;
}

int getSum(Node * tmp)
{
    if (!tmp)   return 0;
    if (tmp->data != tmp->count)
        result += tmp->count;
    getSum(tmp->left);
    getSum(tmp->right);
}

int main()
{

    int n;
    cin >> n;

    Node * arr = new Node[n];
    for (int i = 0; i < n; i++)
    {
        cin >> arr[i].data;
        arr[i].count = arr[i].data;
    }

    priority_queue<Node *, vector<Node *>, cmp> q;
    for (int i = 0; i < n; i++)
        q.push(arr + i);
    Node * tmp = NULL;
    Node * tmp1 = NULL;
    Node * tmp2 = NULL;
    while (!q.empty())
    {
        tmp1 = q.top();
        q.pop();
        if (q.empty())
        {
            tmp = tmp1;
            break;
        }
        tmp2 = q.top();
        q.pop();
        tmp = CreateHuffman(tmp1, tmp2);
        q.push(tmp);
    }

    getSum(tmp);
    cout << result << endl;

    return 0;
}

