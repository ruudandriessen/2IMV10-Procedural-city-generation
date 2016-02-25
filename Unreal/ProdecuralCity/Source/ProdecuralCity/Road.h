#pragma once

#include "Components/SplineMeshComponent.h"

#include "GameFramework/Actor.h"
#include "Road.generated.h"

class USplineComponent;

UCLASS()
class PRODECURALCITY_API ARoad : public AActor
{
	GENERATED_UCLASS_BODY()
	
public:
    UPROPERTY(EditAnywhere, BlueprintReadWrite)
    USplineMeshComponent* RoadSpline;
    
	// Sets default values for this actor's properties
	ARoad();

	// Called when the game starts or when spawned
	virtual void BeginPlay() override;
	
	// Called every frame
	virtual void Tick( float DeltaSeconds ) override;
	
};
