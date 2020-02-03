// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.
/*===========================================================================
	Generated code exported from UnrealHeaderTool.
	DO NOT modify this manually! Edit the corresponding .h files instead!
===========================================================================*/

#include "UObject/GeneratedCppIncludes.h"
#include "WaveFunctionCollapse/WaveFunctionCollapseGameModeBase.h"
#ifdef _MSC_VER
#pragma warning (push)
#pragma warning (disable : 4883)
#endif
PRAGMA_DISABLE_DEPRECATION_WARNINGS
void EmptyLinkFunctionForGeneratedCodeWaveFunctionCollapseGameModeBase() {}
// Cross Module References
	WAVEFUNCTIONCOLLAPSE_API UClass* Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_NoRegister();
	WAVEFUNCTIONCOLLAPSE_API UClass* Z_Construct_UClass_AWaveFunctionCollapseGameModeBase();
	ENGINE_API UClass* Z_Construct_UClass_AGameModeBase();
	UPackage* Z_Construct_UPackage__Script_WaveFunctionCollapse();
// End Cross Module References
	void AWaveFunctionCollapseGameModeBase::StaticRegisterNativesAWaveFunctionCollapseGameModeBase()
	{
	}
	UClass* Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_NoRegister()
	{
		return AWaveFunctionCollapseGameModeBase::StaticClass();
	}
	struct Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_Statics
	{
		static UObject* (*const DependentSingletons[])();
#if WITH_METADATA
		static const UE4CodeGen_Private::FMetaDataPairParam Class_MetaDataParams[];
#endif
		static const FCppClassTypeInfoStatic StaticCppClassTypeInfo;
		static const UE4CodeGen_Private::FClassParams ClassParams;
	};
	UObject* (*const Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_Statics::DependentSingletons[])() = {
		(UObject* (*)())Z_Construct_UClass_AGameModeBase,
		(UObject* (*)())Z_Construct_UPackage__Script_WaveFunctionCollapse,
	};
#if WITH_METADATA
	const UE4CodeGen_Private::FMetaDataPairParam Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_Statics::Class_MetaDataParams[] = {
		{ "HideCategories", "Info Rendering MovementReplication Replication Actor Input Movement Collision Rendering Utilities|Transformation" },
		{ "IncludePath", "WaveFunctionCollapseGameModeBase.h" },
		{ "ModuleRelativePath", "WaveFunctionCollapseGameModeBase.h" },
		{ "ShowCategories", "Input|MouseInput Input|TouchInput" },
	};
#endif
	const FCppClassTypeInfoStatic Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_Statics::StaticCppClassTypeInfo = {
		TCppClassTypeTraits<AWaveFunctionCollapseGameModeBase>::IsAbstract,
	};
	const UE4CodeGen_Private::FClassParams Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_Statics::ClassParams = {
		&AWaveFunctionCollapseGameModeBase::StaticClass,
		nullptr,
		&StaticCppClassTypeInfo,
		DependentSingletons,
		nullptr,
		nullptr,
		nullptr,
		ARRAY_COUNT(DependentSingletons),
		0,
		0,
		0,
		0x009002A8u,
		METADATA_PARAMS(Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_Statics::Class_MetaDataParams, ARRAY_COUNT(Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_Statics::Class_MetaDataParams))
	};
	UClass* Z_Construct_UClass_AWaveFunctionCollapseGameModeBase()
	{
		static UClass* OuterClass = nullptr;
		if (!OuterClass)
		{
			UE4CodeGen_Private::ConstructUClass(OuterClass, Z_Construct_UClass_AWaveFunctionCollapseGameModeBase_Statics::ClassParams);
		}
		return OuterClass;
	}
	IMPLEMENT_CLASS(AWaveFunctionCollapseGameModeBase, 3364655304);
	template<> WAVEFUNCTIONCOLLAPSE_API UClass* StaticClass<AWaveFunctionCollapseGameModeBase>()
	{
		return AWaveFunctionCollapseGameModeBase::StaticClass();
	}
	static FCompiledInDefer Z_CompiledInDefer_UClass_AWaveFunctionCollapseGameModeBase(Z_Construct_UClass_AWaveFunctionCollapseGameModeBase, &AWaveFunctionCollapseGameModeBase::StaticClass, TEXT("/Script/WaveFunctionCollapse"), TEXT("AWaveFunctionCollapseGameModeBase"), false, nullptr, nullptr, nullptr);
	DEFINE_VTABLE_PTR_HELPER_CTOR(AWaveFunctionCollapseGameModeBase);
PRAGMA_ENABLE_DEPRECATION_WARNINGS
#ifdef _MSC_VER
#pragma warning (pop)
#endif
