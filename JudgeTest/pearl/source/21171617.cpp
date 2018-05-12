#include <stdio.h>
#include <stdlib.h>
struct tree
{
	int weight;
	struct tree* left;
	struct tree* right;
};
int compare(const void* a,const void* b)
{
	return ((tree*)a)->weight-((tree*)b)->weight;
}
int main(void)
{
	int i,j,n,sum=0;
	struct tree* t[30001];
	struct tree* p;
	scanf("%d",&n);
	for(i=0;i<n;i++)
	{
		t[i]=(struct tree*)malloc(sizeof(struct tree));
		scanf("%d",&t[i]->weight);
	}
	qsort(t,n,sizeof(struct tree*),compare);
	for(i=0;i<n-1;i++)
	{
		p=(struct tree*)malloc(sizeof(struct tree));
		p->weight=t[i]->weight+t[i+1]->weight;
		for(j=i+2;j<n;j++)
		{
			if(p->weight>t[j]->weight)
			t[j-1]=t[j];
			else break;
		}
		t[j-1]=p;
	}
	printf("%d",t[n-1]->weight);
	fclose(stdin);
	fclose(stdin);
}
