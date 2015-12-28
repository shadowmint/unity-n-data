# n-data

Data serialization utility functions.

## Usage

See the tests in the `Editor/` folder for each class for usage examples.

For full scene serializataion see `SceneDataTests.cs`; typical usage would
be similar to:

    using N.Package.Data.Scene;
    using N.Package.Data;

    var instance = new SceneData().Serialize();
    var serialized = Json.Serialize(instance);

    ...

    var thawed = Json.Deserialize<SceneData>(serialized).Unwrap();
    thawed.Deserialize();
    thawed.Destroy(true);

NB. That only objects tagged with the `SceneFabricated` component are serialized,
and certain serialization limitations (eg. references) apply.

## Install

From your unity project folder:

    npm init
    npm install shadowmint/unity-n-core --save
    echo Assets/packages >> .gitignore
    echo Assets/packages.meta >> .gitignore

The package and all its dependencies will be installed in
your Assets/packages folder.

## Development

Setup and run tests:

    npm install
    npm install ..
    cd test
    npm install
    gulp

Remember that changes made to the test folder are not saved to the package
unless they are copied back into the source folder.

To reinstall the files from the src folder, run `npm install ..` again.

### Tests

All tests are wrapped in `#if ...` blocks to prevent test spam.

You can enable tests in: Player settings > Other Settings > Scripting Define Symbols

The test key for this package is: N_DATA_TESTS
