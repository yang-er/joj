#include<cstdio>
#include<cstdlib>
#include<queue>
#include<vector>
using namespace std;

int main()
{
    int i,n,ztk,ktz;
    scanf("%d", &n);
    priority_queue<int, vector<int>, greater<int> > huffman;
    int sum = 0;
    for (i = 0; i < n; i++)
    {
        scanf("%d", &sum);
        huffman.push(sum);
    }

    sum = 0;
    while (huffman.size() != 1)
    {
        ztk = huffman.top();
        huffman.pop();
        ktz = huffman.top();
        huffman.pop();
        huffman.push(ztk + ktz);
        sum = sum + ztk + ktz;
    }
    printf("%d\n", sum);
    return 0;
}
