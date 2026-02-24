import pytest
from helpers.app import launch_app, login


@pytest.fixture(scope="session")
def app():
    """Launch the app once for the entire test session."""
    application = launch_app()
    yield application
    application.kill()


@pytest.fixture(scope="session")
def main_window(app):
    """
    Log in once and return the main window for the whole session.
    Use this fixture in any test that needs the app already logged in.
    """
    window = login(app)
    yield window


@pytest.fixture(scope="function")
def fresh_app():
    """
    Launch a brand-new app instance for tests that need a clean slate
    (e.g. login tests that test bad credentials or logout).
    Kills the app after each test.
    """
    application = launch_app()
    yield application
    application.kill()
