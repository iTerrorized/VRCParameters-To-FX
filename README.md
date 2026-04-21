# VRCParameters To FX

A Unity editor tool that syncs all parameters from a `VRCExpressionParameters` asset into an FX `AnimatorController` with the correct types.

## Installation

Install via the [VRChat Creator Companion](https://vcc.docs.vrchat.com/) using the [Terrorized VPM Listing](https://vpm.terrorized.dev).

## Usage

1. Open the tool via **Terrorized → Tools → VRCParameters To FX**
2. Assign your **VRC Parameters** asset
3. Assign your **FX Controller** (AnimatorController)
4. Click **Add Parameters to FX**

The tool will add any missing parameters and update the type and default value of any that already exist. Separator entries (names containing `-----`) are automatically ignored.

## Requirements

- Unity 2022.3.22f1+
- VRChat Avatar SDK 3.10.0+
