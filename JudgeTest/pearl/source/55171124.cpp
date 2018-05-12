#include<iostream>
#include<vector>
#include<assert.h>
#include <stdio.h>
#include <stdlib.h>
using namespace std;
template<class T,class Continer>
class Heap
{
public:
    Heap(){};
    void Push(const T& x);
    void Pop();
    T Top();
    bool Empty();
    size_t Size();
    void AdjustDown(size_t parent);
    void AdjustUp(int child);
private:
    vector<T> node;
};

template<class T, class Continer>
void Heap<T, Continer>::Push(const T& x)
{
    node.push_back(x);
    AdjustUp(node.size() - 1);
}
template<class T, class Continer>
void Heap<T, Continer>::Pop()
{
    assert(!node.empty());
    size_t size = node.size();
    swap(node[0], node[size - 1]);
    node.pop_back();
    AdjustDown(0);
}
template<class T, class Continer>
T Heap<T, Continer>::Top()
{
    return node[0];
}
template<class T, class Continer>
bool Heap<T, Continer>::Empty()
{
    return node.empty();
}
template<class T, class Continer>
size_t Heap<T, Continer>::Size()
{
    return node.size();
}

template<class T, class Continer>
void Heap<T, Continer>::AdjustDown(size_t parent)
{
    Continer con;
    size_t child = parent * 2 + 1;
    size_t size = node.size();
    while (child < size)
    {
        if (child + 1 < size&&(node[child + 1]<node[child]))

            ++child;
        if ((node[child]<node[parent]))
        {
            swap(node[parent], node[child]);
            parent = child;
            child = parent * 2 + 1;
        }
        else
            break;
    }
}
template<class T, class Continer >
void Heap<T, Continer>::AdjustUp(int child)
{
    Continer con;
    int parent = (child - 1) / 2;
    while (child > 0)
    {
        if ((node[child]<node[parent]))
        {
            swap(node[child], node[parent]);
            child = parent;
            parent = (child - 1) / 2;
        }
        else
            break;
    }
}


struct Less{};
int main()
{
	int n,tmp=0,sum=0,q;
	Heap<int,Less> hp;
	int a[30010];
	scanf("%d",&n);
	for(int i=0;i<n;i++)
    {
        scanf("%d",&a[i]);
        hp.Push(a[i]);
    }
    tmp=hp.Top();
        hp.Pop();
    for(int j=0;j<n-1;j++)
    {
        q=hp.Top();
        hp.Pop();
        tmp=tmp+q;
        sum=sum+tmp;
    }

    cout<<sum<<endl;
	fclose(stdin);
	fclose(stdout);
    return 0;
}
int cmp(const void *a,const void *b)
{
    return *(int *)a-*(int *)b;
}
