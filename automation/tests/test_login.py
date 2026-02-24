import time
import pytest
from helpers.app import get_window, login


def test_empty_fields_shows_error(fresh_app):
    """Clicking Sign In with no credentials should show an error."""
    win = get_window(fresh_app)
    win.child_window(title="Sign In", control_type="Button").click()
    time.sleep(0.3)

    # _lblError is the third Label in the card
    error = win.child_window(title="Invalid username or password", control_type="Text")
    assert error.exists(), "Error label should appear for empty credentials"


def test_wrong_credentials_shows_error(fresh_app):
    """Wrong username/password should show the invalid credentials error."""
    win = get_window(fresh_app)
    win.child_window(class_name="TextBox", found_index=0).set_edit_text("baduser")
    win.child_window(class_name="TextBox", found_index=1).set_edit_text("badpass")
    win.child_window(title="Sign In", control_type="Button").click()
    time.sleep(0.3)

    error = win.child_window(title="Invalid username or password", control_type="Text")
    assert error.exists()


def test_valid_login_shows_nav(fresh_app):
    """Valid credentials should dismiss the login card and show the nav bar."""
    login(fresh_app)
    win = get_window(fresh_app)

    # All four nav buttons should now be present
    assert win.child_window(title="Dashboard", control_type="Button").exists()
    assert win.child_window(title="Accounts", control_type="Button").exists()
    assert win.child_window(title="Transactions", control_type="Button").exists()
    assert win.child_window(title="Transfer", control_type="Button").exists()


def test_logout_returns_to_login(fresh_app):
    """Clicking Logout should bring the login form back."""
    login(fresh_app)
    win = get_window(fresh_app)
    win.child_window(title="Logout", control_type="Button").click()
    time.sleep(0.5)

    # Sign In button should reappear
    assert win.child_window(title="Sign In", control_type="Button").exists()
