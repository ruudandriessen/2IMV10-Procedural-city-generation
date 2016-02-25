#include "ProdecuralCity.h"
#include "Road.h"


// Sets default values
ARoad::ARoad(const FObjectInitializer& ObjectInitializer) : Super (ObjectInitializer) {
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
    
    RootComponent = ObjectInitializer.CreateDefaultSubobject<USceneComponent>(this, TEXT("CT-Root"));
    RoadSpline = ObjectInitializer.CreateDefaultSubobject<USplineMeshComponent>(this, TEXT("SplineComp"));

}

// Called when the game starts or when spawned
void ARoad::BeginPlay() {
	Super::BeginPlay();
	
}

// Called every frame
void ARoad::Tick( float DeltaTime ) {
    Super::Tick( DeltaTime );
//    FVector NewLocation = GetActorLocation();
//    float DeltaHeight = (FMath::Sin(RunningTime + DeltaTime) - FMath::Sin(RunningTime));
//    NewLocation.Z += DeltaHeight * 20.0f;       //Scale our height by a factor of 20
//    RunningTime += DeltaTime;
//    SetActorLocation(NewLocation);

}

//void ARoad::AddSpline() {
//    int32 index = RoadSplines.Add(CreateDefaultSubobject<USplineComponent>(this, TEXT("SplineComp")));
//    RoadSplines[index]->RegisterComponent();
//    RoadSplines[index]->SetActive(true);
//    RoadSplines[index]->SetMobility(EComponentMobility::Movable);
//    RoadSplines[index]->AttachTo(RootComponent);
//    RoadSplines[index]->bSplineHasBeenEdited = true;
//}

