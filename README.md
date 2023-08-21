# Airac.NET - Library for caclulationg AIRAC Cycle data

[![Apache License v2.0](https://img.shields.io/badge/license-Apache%20License%202.0-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0.txt)
## Overview

Per [Wikipedia](https://en.wikipedia.org/wiki/Aeronautical_Information_Publication):

> AIPs (Aeronautical Information Publications) are kept up-to-date by regular revision on a fixed cycle. For operationally significant changes in information, the cycle known as the AIRAC (Aeronautical Information Regulation And Control) cycle, first introduced in 1964, is used: revisions are produced every 56 days (double AIRAC cycle) or every 28 days (single AIRAC cycle). 

The mechanics of a 28 day cycle dictate that most calendar years will contain 13 cycles.  However, misalignment with the calendar results in some years (e.g. 2020) containing 14 cycles.

### Usage

Given a `Date` or `DateTime`, `Cycle` can provide the Effective Date and identifier of the cycle enclosing said date.  Additionally, `Cycle.FromIdentifier` will construct a representation of the cycle associated with that identifier.

### Notes

Data sourced from professional vendors (Garmin, Jeppesen, Navigraph,  etc) will have the correct cycle info associated with it.  I wrote this merely to aid in exporting flight plan data from [SkyVector](https://skyvector.com/) to [X-Plane](https://www.x-plane.com/)

### Credits

Inspired by the [go](https://github.com/wjkohnen/airac/) and [java](https://github.com/jwkohnen/airac-java/) libraries by @jwkohnen 